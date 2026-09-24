using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AirlineBookingSystem.Payment.Application.Command
{
    public record RefundPaymentCommand(Guid PaymentId) : IRequest;
}
