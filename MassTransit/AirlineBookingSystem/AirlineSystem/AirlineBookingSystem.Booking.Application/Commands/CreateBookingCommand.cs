using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AirlineBookingSystem.Booking.Application.Commands
{
    public record CreateBookingCommand(Guid FlightId, string passengerName, string SeatNumber) : IRequest<Guid>;
 
}
