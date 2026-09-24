using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AirlineBookingSystem.Notification.Application.Commands
{
    public record SendNotificationCommands(string Recipient, string Messagge, string Type) : IRequest;
}
