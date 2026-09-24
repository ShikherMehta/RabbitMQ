using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AirlineBookingSystem.Flights.Application.Commands
{
    public record DeleteFlightCommand(Guid Id) : IRequest;
}
