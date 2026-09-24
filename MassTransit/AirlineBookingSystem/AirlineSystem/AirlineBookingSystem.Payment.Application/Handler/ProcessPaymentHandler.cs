using AirlineBookingSystem.Payment.Application.Command;
using AirlineBookingSystem.Payment.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using MassTransit;
using AirlineBookingSystem.BuildingBlocks.Contracts;
namespace AirlineBookingSystem.Payment.Application.Handler
{
    public class ProcessPaymentHandler : IRequestHandler<ProcessPaymentCommand, Guid>
    {
        private readonly IPaymentRepository _PaymentRepository;
        private readonly IPublishEndpoint _publishEndPoints;
        public ProcessPaymentHandler(IPaymentRepository PaymentRepository, IPublishEndpoint publishEndpoint)
        {
            _PaymentRepository = PaymentRepository;
            _publishEndPoints = publishEndpoint;
        }
        public async Task<Guid> Handle(ProcessPaymentCommand request, CancellationToken cancellationToken) 
        {
            var payment = new AirlineBookingSystem.Payment.Core.Entities.Payment
            {
                Id = Guid.NewGuid(),
                BookingId =request.BookingId,
                Amount = request.Amount,
                PaymentDate = DateTime.UtcNow
            };
            await _PaymentRepository.ProcessPaymentAsync(payment);
            // Publish PaymentProcessedEvent
            await _publishEndPoints.Publish(new PaymentProcessEvent(
                payment.Id,
                payment.BookingId,
                payment.Amount,
                payment.PaymentDate
                ));
            return payment.Id;
        }
    }
}
