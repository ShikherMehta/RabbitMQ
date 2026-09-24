using AirlineBookingSystem.BuildingBlocks.Contracts;
using AirlineBookingSystem.Notification.Application.Commands;
using MassTransit;
using MediatR;

namespace AirlineBookingSystem.Notification.Application.Consumer
{
    public class PaymentProcessedConsumer : IConsumer<PaymentProcessEvent>
    {
        private IMediator _mediator;
        public PaymentProcessedConsumer(IMediator mediator) 
        {
            _mediator = mediator;
        }
        public async Task Consume(ConsumeContext<PaymentProcessEvent> context)
        {
            var paymentProcessedEvent = context.Message;
            var message = $"Payment of ${paymentProcessedEvent.Amount} for Booking ID: {paymentProcessedEvent.BookingId}";
            var command = new SendNotificationCommands("someone@gmail.com",message,"Email");
            await _mediator.Send(command);
        }
    }
}
