using InventoryService.Data;
using InventoryService.Event;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace InventoryService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    private IConnection? _connection;
    private IChannel? _channel;

    public Worker(
        ILogger<Worker> logger,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        // Connect to RabbitMQ
        var factory = new ConnectionFactory
        {
            HostName = "localhost",
            Port = 5672,
            UserName = "admin",
            Password = "admin123"
        };

        _connection = await factory.CreateConnectionAsync();

        _channel = await _connection.CreateChannelAsync();

        // Declare exchange
        await _channel.ExchangeDeclareAsync(
            exchange: "OrderServiceExchange",
            type: ExchangeType.Fanout
            );

        // Bind InventoryQueue to OrderServiceExchange
        await _channel.QueueBindAsync(
            queue: "OrderServiceQueue",
            exchange: "OrderServiceExchange",
            routingKey: "");

        // Process one message at a time
        await _channel.BasicQosAsync(
            prefetchSize: 0,
            prefetchCount: 1,
            global: false);

        // Create consumer
        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += async (sender, args) =>
        {
            try
            {
                // Read message
                var body = args.Body.ToArray();

                var json = Encoding.UTF8.GetString(body);
                
                _logger.LogInformation(
                    "Message received: {Message}",
                    json);

                // Deserialize message
                var order =
                    JsonSerializer.Deserialize<OrderCreatedEvent>(
                        json);

                if (order == null)
                {
                    _logger.LogError(
                        "Unable to deserialize OrderCreatedEvent.");

                    await _channel.BasicNackAsync(
                        args.DeliveryTag,
                        multiple: false,
                        requeue: false);

                    return;
                }
                await Task.Delay(10000);


                // Update inventory
                await UpdateInventory(order);

                // Tell RabbitMQ processing succeeded
                await _channel.BasicAckAsync(
                    args.DeliveryTag,
                    multiple: false);

                _logger.LogInformation(
                    "Inventory updated successfully. OrderId: {OrderId}"
                    );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error processing inventory message.");

                // Processing failed
                await _channel.BasicNackAsync(
                    args.DeliveryTag,
                    multiple: false,
                    requeue: false);
            }
        };

        // Start consuming
        await _channel.BasicConsumeAsync(
            queue: "OrderServiceQueue",
            autoAck: false,
            consumer: consumer);

        _logger.LogInformation(
            "Inventory Service started. Waiting for messages...");

        // Keep BackgroundService alive
        await Task.Delay(
            Timeout.Infinite,
            stoppingToken);
    }

    private async Task UpdateInventory(
        OrderCreatedEvent order)
    {
        using var scope = _scopeFactory.CreateScope();

        var db = scope.ServiceProvider
            .GetRequiredService<InventoryDbContext>();

        // Find inventory for the product
        var inventory = await db.Inventories
            .SingleOrDefaultAsync(
                x => x.ProductId == order.ProductId);

        if (inventory == null)
        {
            throw new InvalidOperationException(
                $"Inventory not found for ProductId: {order.ProductId}");
        }

        // Check available stock
        if (inventory.AvailableQuantity < order.Quantity)
        {
            throw new InvalidOperationException(
                $"Insufficient inventory for ProductId: {order.ProductId}");
        }

        // Reserve inventory
        inventory.AvailableQuantity -= order.Quantity;

        inventory.ReservedQuantity += order.Quantity;

        inventory.UpdatedAt = DateTime.UtcNow;

        await db.SaveChangesAsync();

        _logger.LogInformation(
            "Product {ProductId}: Reserved {Quantity}. " +
            "Available quantity now: {AvailableQuantity}",
            order.ProductId,
            order.Quantity,
            inventory.AvailableQuantity);
    }

    public override async Task StopAsync(
        CancellationToken cancellationToken)
    {
        if (_channel != null)
        {
            await _channel.CloseAsync();
        }

        if (_connection != null)
        {
            await _connection.CloseAsync();
        }

        await base.StopAsync(cancellationToken);
    }
}