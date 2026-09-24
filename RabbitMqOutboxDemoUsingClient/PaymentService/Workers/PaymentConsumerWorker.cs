using Microsoft.EntityFrameworkCore;
using PaymentService.Data;
using PaymentService.Model;
using PaymentService.RabbitMq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
namespace PaymentService.Workers
{
    public class PaymentConsumerWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly RabbitMqConnection _rabbitMq;

        public PaymentConsumerWorker(
            IServiceScopeFactory scopeFactory,
            RabbitMqConnection rabbitMq)
        {
            _scopeFactory = scopeFactory;
            _rabbitMq = rabbitMq;
        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            var connection =
                await _rabbitMq.GetConnectionAsync();

            var channel =
                await connection.CreateChannelAsync();

            // Configure exchanges, queues and bindings
            await RabbitMqTopology.ConfigureAsync(channel);

            // Process one message at a time
            await channel.BasicQosAsync(
                prefetchSize: 0,
                prefetchCount: 1,
                global: false);

            var consumer =
                new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync +=
                async (sender, args) =>
                {
                    await ProcessMessage(
                        channel,
                        args);
                };

            await channel.BasicConsumeAsync(
                queue:
                    RabbitMqTopology.PaymentQueue,

                autoAck:
                    false,

                consumer:
                    consumer);

            Console.WriteLine(
                "Payment consumer started.");

            try
            {
                await Task.Delay(
                    Timeout.Infinite,
                    stoppingToken);
            }
            catch (OperationCanceledException)
            {
                // Application shutting down
            }
        }


        private async Task ProcessMessage(
            IChannel channel,
            BasicDeliverEventArgs args)
        {
            try
            {
                // =========================================
                // 1. READ MESSAGE
                // =========================================

                var json =
                    Encoding.UTF8.GetString(
                        args.Body.ToArray());

                Console.WriteLine(
                    $"Received message: {json}");


                // =========================================
                // 2. READ RETRY COUNT
                // =========================================

                int retryCount = 0;

                if (args.BasicProperties.Headers != null &&
                    args.BasicProperties.Headers.TryGetValue(
                        "x-retry-count",
                        out var value))
                {
                    retryCount =
                        Convert.ToInt32(value);
                }

                Console.WriteLine(
                    $"Current retry count: {retryCount}");


                // =========================================
                // 3. DESERIALIZE MESSAGE
                // =========================================

                var order =
                    JsonSerializer.Deserialize<OrderCreatedEvent>(
                        json);

                if (order == null)
                {
                    throw new Exception(
                        "Invalid OrderCreated message.");
                }


                // =========================================
                // 4. DATABASE OPERATION
                // =========================================

                using var scope =
                    _scopeFactory.CreateScope();

                var db =
                    scope.ServiceProvider
                        .GetRequiredService<AppDbContext>();


                // =========================================
                // 5. IDEMPOTENCY CHECK
                // =========================================

                var existingPayment =
                    await db.Payments
                        .FirstOrDefaultAsync(
                            x => x.OrderId == order.OrderId);

                if (existingPayment != null)
                {
                    Console.WriteLine(
                        $"Payment already exists for OrderId {order.OrderId}");

                    await channel.BasicAckAsync(
                        args.DeliveryTag,
                        multiple: false);

                    return;
                }


                // =========================================
                // 6. SIMULATED FAILURE
                // =========================================

                // Use Amount = 9999 to test retry
                if (order.Amount == 9999)
                {
                    throw new Exception(
                        "Simulated payment failure.");
                }


                // =========================================
                // 7. CREATE PAYMENT
                // =========================================

                var payment =
                    new Payment
                    {
                        Id = Guid.NewGuid(),

                        OrderId =
                            order.OrderId,

                        Amount =
                            order.Amount,

                        Status =
                            "Completed",

                        CreatedAt =
                            DateTime.UtcNow
                    };

                db.Payments.Add(payment);

                await db.SaveChangesAsync();


                // =========================================
                // 8. SUCCESS → ACK
                // =========================================

                await channel.BasicAckAsync(
                    args.DeliveryTag,
                    multiple: false);

                Console.WriteLine(
                    $"Payment completed for OrderId {order.OrderId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Payment processing failed: {ex.Message}");

                // =========================================
                // READ RETRY COUNT AGAIN
                // =========================================

                int retryCount = 0;

                if (args.BasicProperties.Headers != null &&
                    args.BasicProperties.Headers.TryGetValue(
                        "x-retry-count",
                        out var value))
                {
                    retryCount =
                        Convert.ToInt32(value);
                }


                // =========================================
                // RETRY
                // =========================================

                if (retryCount < 3)
                {
                    int nextRetryCount =
                        retryCount + 1;

                    Console.WriteLine(
                        $"Sending message to retry queue. " +
                        $"Retry #{nextRetryCount}");


                    var headers =
                        new Dictionary<string, object?>
                        {
                            ["x-retry-count"] =
                                nextRetryCount
                        };


                    var properties =
                        new BasicProperties
                        {
                            Persistent = true,

                            MessageId =
                                args.BasicProperties.MessageId,

                            Type =
                                args.BasicProperties.Type,

                            Headers =
                                headers
                        };


                    // Publish to Retry Exchange

                    await channel.BasicPublishAsync(
                        exchange:
                            RabbitMqTopology.PaymentRetryExchange,

                        routingKey:
                            RabbitMqTopology.PaymentRetryRoutingKey,

                        mandatory:
                            true,

                        basicProperties:
                            properties,

                        body:
                            args.Body.ToArray());


                    // IMPORTANT:
                    // ACK original message after
                    // successfully publishing it
                    // to retry exchange.

                    await channel.BasicAckAsync(
                        args.DeliveryTag,
                        multiple: false);
                }
                else
                {
                    // =====================================
                    // MAX RETRIES REACHED
                    // =====================================

                    Console.WriteLine(
                        "Maximum retry attempts reached.");

                    // false = don't acknowledge multiple messages
                    // false = don't requeue

                    await channel.BasicNackAsync(
                        args.DeliveryTag,

                        multiple: false,

                        requeue: false);
                }
            }
        }
    }


    // =============================================
    // EVENT CONTRACT
    // =============================================

    public class OrderCreatedEvent
    {
        public string MessageId { get; set; } = "";

        public string EventType { get; set; } = "";

        public Guid OrderId { get; set; }

        public string CustomerName { get; set; } = "";

        public decimal Amount { get; set; }
    }
}