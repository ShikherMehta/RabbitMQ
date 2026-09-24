using System;
using System.Collections.Generic;
using System.Text;

namespace AirlineBookingSystem.Booking.Core.Entities
{
    public class Booking
    {
        public Guid Id{ get; set; }
        public Guid FlightId { get; set; }
        public string PassengerName { get; set; } = string.Empty;
        public string SeatNumber { get; set; } = string.Empty;
        public DateTime BookingDate { get; set; }
    }
}
