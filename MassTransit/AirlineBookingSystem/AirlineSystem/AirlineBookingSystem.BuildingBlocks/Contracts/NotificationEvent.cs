using System;
namespace AirlineBookingSystem.BuildingBlocks.Contracts
{
    public record NotificationEvent(string recipient, string message, string type);
}


