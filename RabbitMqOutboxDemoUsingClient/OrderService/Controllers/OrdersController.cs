using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Models;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _db;

        public OrdersController(AppDbContext db)
        {
            _db = db;
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            CreateOrderRequest request)
        {
            using var transaction =
                await _db.Database.BeginTransactionAsync();

            var order = new Order
            {
                Id = Guid.NewGuid(),
                CustomerName = request.CustomerName,
                Amount = request.Amount,
                Status = "Created",
                CreatedAt = DateTime.UtcNow
            };

            _db.Orders.Add(order);

            var eventPayload = new
            {
                MessageId = Guid.NewGuid(),
                EventType = "OrderCreated",
                OrderId = order.Id,
                CustomerName = order.CustomerName,
                Amount = order.Amount
            };

            var outbox = new OutboxMessage
            {
                Id = Guid.NewGuid(),

                EventType = "OrderCreated",

                Payload =
                    JsonSerializer.Serialize(eventPayload),

                CreatedAt = DateTime.UtcNow,

                RetryCount = 0
            };

            _db.OutboxMessages.Add(outbox);

            await _db.SaveChangesAsync();

            await transaction.CommitAsync();

            return Ok(new
            {
                order.Id,
                Message = "Order created and event stored in Outbox"
            });
        }
    }

    public record CreateOrderRequest(
        string CustomerName,
        decimal Amount);
}
