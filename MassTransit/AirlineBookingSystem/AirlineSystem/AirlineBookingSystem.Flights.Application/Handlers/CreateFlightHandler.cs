using AirlineBookingSystem.Flights.Application.Commands;
using AIrlineBookingSystem.Flights.Core.Entities;
using AIrlineBookingSystem.Flights.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AirlineBookingSystem.Flights.Application.Handlers
{
    public class CreateFlightHandler  :IRequestHandler<CreateFlighCommand,Guid>
    {
        private readonly IFlightRepository _flightRepository;
        public CreateFlightHandler(IFlightRepository flightRepository) 
        {
            _flightRepository = flightRepository;
        }
        public async Task<Guid> Handle(CreateFlighCommand request, CancellationToken cancellationToken)
        {
            var flight = new Flight
            {
                Id = Guid.NewGuid(),
                FlightNumber = request.FlightNumber,
                Origin = request.Origin,
                Destination = request.Destination,
                DepartureTime = request.DepartureTime,
                ArrivateTime = request.ArrivalTime
            };

            await _flightRepository.AddFlightAsync(flight);
            return flight.Id;
        }
    }
}
