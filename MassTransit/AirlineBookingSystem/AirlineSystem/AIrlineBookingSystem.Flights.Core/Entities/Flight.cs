using System;
using System.Collections.Generic;
using System.Text;

namespace AIrlineBookingSystem.Flights.Core.Entities
{
    public class Flight
    {
        public Guid Id { get; set; }
        public string FlightNumber { get; set; } = string.Empty;
        public string Origin { get; set; }= string.Empty;
        public string Destination { get; set; } = string.Empty;
        public DateTime ArrivateTime { get; set; }
        public DateTime DepartureTime { get; set; }
    }
}
