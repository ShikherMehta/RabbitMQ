namespace AirlineBookingSystem.BuildingBlocks.Contracts
{
    public record FlightBookedEvent(Guid BookingId, Guid FlightId, string PassengerName, string SeatNumber, DateTime BookingDate);
}