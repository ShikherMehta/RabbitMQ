using AIrlineBookingSystem.Flights.Core.Entities;
using AIrlineBookingSystem.Flights.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AirlineBookingSystem.Flights.Application.Queries
{
    public class GetAllFlightHandler : IRequestHandler<GetAllFlightsQuery, IEnumerable<Flight>>
    {
        private readonly IFlightRepository _repository;
        public GetAllFlightHandler(IFlightRepository repository) 
        {
            _repository = repository;
        }
        
        public async Task<IEnumerable<Flight>> Handle(GetAllFlightsQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetFlightsAsync();
        }
    }
}
