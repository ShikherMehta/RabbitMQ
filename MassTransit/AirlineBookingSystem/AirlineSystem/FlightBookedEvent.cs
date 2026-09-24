using System;

public record FlightBookedEvent(Guid BookingId, string PassengerName, string SeatNumber, DateTime BookingDate);
