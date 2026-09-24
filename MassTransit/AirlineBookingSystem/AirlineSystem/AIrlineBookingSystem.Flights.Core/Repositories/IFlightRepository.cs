using AIrlineBookingSystem.Flights.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AIrlineBookingSystem.Flights.Core.Repositories
{
    public interface IFlightRepository
    {
        Task<IEnumerable<Flight>> GetFlightsAsync();
        Task AddFlightAsync(Flight flight);
        Task DeleteFlightAsync(Guid Id);
    }
}
