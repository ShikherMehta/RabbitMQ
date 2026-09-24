using AIrlineBookingSystem.Flights.Core.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AirlineBookingSystem.Flights.Application.Queries
{
    public record GetAllFlightsQuery : IRequest<IEnumerable<Flight>>;
}
