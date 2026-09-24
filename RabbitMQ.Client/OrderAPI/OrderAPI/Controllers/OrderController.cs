using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderAPI.Model;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace OrderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderDbContext _context;
        public OrderController(OrderDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrder(Order order)
        {
            order.Status = "Created";
            order.CreatedAt = DateTime.UtcNow;
            _context.Orders.Add(order);

            await _context.SaveChangesAsync();
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "admin",
                Password = "admin123"
            };

            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: "OrderServiceQueue", durable: true, exclusive: false, autoDelete: false,
                             arguments: new Dictionary<string, object?> { { "x-queue-type", "quorum" } });

            await channel.ExchangeDeclareAsync(exchange: "OrderServiceExchange", type: ExchangeType.Fanout);
            await channel.QueueBindAsync(queue: "OrderServiceQueue", exchange: "OrderServiceExchange", routingKey: "");
            var message = new Order
            {
                OrderId = order.OrderId,
                CustomerId = order.CustomerId,
                ProductId = order.ProductId,
                Quantity = order.Quantity
            };

            var json = JsonSerializer.Serialize(message);

            var body = Encoding.UTF8.GetBytes(json);

            await channel.BasicPublishAsync(
                exchange: "OrderServiceExchange",
                routingKey: "",
                body: body);
            
            await channel.BasicPublishAsync(exchange: "OrderServiceExchange", routingKey: string.Empty, body: body);

            return Ok(order);

        }     
    }
}
