using AirlineBookingSystem.BuildingBlocks.Contracts;
using AirlineBookingSystem.Payment.Application.Command;
using MassTransit;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AirlineBookingSystem.Payment.Application.Consumer
{
    public class FlightBookedConsumer : IConsumer<FlightBookedEvent>
    {
        private readonly IMediator _mediator;
        public FlightBookedConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task Consume(ConsumeContext<FlightBookedEvent> context)
        {
            var flightBookedEvent = context.Message;
            var command = new ProcessPaymentCommand(flightBookedEvent.BookingId, 10000);
            await _mediator.Send(command);
        }
    }
}
