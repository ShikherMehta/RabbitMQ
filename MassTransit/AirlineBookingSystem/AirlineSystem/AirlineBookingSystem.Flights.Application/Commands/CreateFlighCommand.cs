using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AirlineBookingSystem.Flights.Application.Commands
{
    public record CreateFlighCommand(string FlightNumber, string Origin, string Destination, DateTime DepartureTime, DateTime ArrivalTime)
        : IRequest<Guid>;
}
