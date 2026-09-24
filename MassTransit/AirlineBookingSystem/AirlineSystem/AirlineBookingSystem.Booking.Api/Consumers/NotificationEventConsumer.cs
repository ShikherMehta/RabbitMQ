using AirlineBookingSystem.BuildingBlocks.Contracts;
using MassTransit;
using MediatR;

namespace AirlineBookingSystem.Booking.Api.Consumers
{
    public class NotificationEventConsumer : IConsumer<NotificationEvent>
    {
        public async Task Consume(ConsumeContext<NotificationEvent> context)
        {
            var notificationEvent = context.Message;
            Console.WriteLine($"Received Notification Event: Recipient={notificationEvent.recipient}, " +
                $"Message={notificationEvent.message}");

            await Task.CompletedTask;
        }
    }
}
