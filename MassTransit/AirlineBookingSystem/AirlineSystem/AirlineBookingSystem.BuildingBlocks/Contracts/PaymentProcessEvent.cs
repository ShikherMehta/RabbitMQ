using System;
namespace AirlineBookingSystem.BuildingBlocks.Contracts
{
    public record PaymentProcessEvent(Guid PaymentId, Guid BookingId, int Amount, DateTime PaymentDate);
}