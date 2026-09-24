using AirlineBookingSystem.Payment.Application.Command;
using AirlineBookingSystem.Payment.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AirlineBookingSystem.Payment.Application.Handler
{
    public class RefundPaymentHandler : IRequestHandler<RefundPaymentCommand>
    {
        private readonly IPaymentRepository _PaymentRepository;

        public RefundPaymentHandler(IPaymentRepository PaymentRepository)
        {
            _PaymentRepository = PaymentRepository;
        }

        public async Task Handle(RefundPaymentCommand request, CancellationToken cancellationToken)
        {
            await _PaymentRepository.RefundpaymentAsync(request.PaymentId);
        }

    }
}
