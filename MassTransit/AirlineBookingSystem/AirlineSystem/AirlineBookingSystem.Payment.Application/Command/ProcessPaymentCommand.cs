using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AirlineBookingSystem.Payment.Application.Command
{
    public record ProcessPaymentCommand(Guid BookingId, int Amount) : IRequest<Guid>;
}
