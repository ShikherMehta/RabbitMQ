using AirlineBookingSystem.Notification.Application.Commands;
using AirlineBookingSystem.Notification.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace AirlineBookingSystem.Notification.Application.Handlers
{
    public class SendNotificationHandler : IRequestHandler<SendNotificationCommands>
    {
        private readonly INotificationService _notificationService;
        public SendNotificationHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;

        }

        public async Task Handle(SendNotificationCommands request, CancellationToken cancellation)
        {
            var notification = new AirlineBookingSystem.Notification.Core.Entities.Notification
            {
                Id = Guid.NewGuid(),
                Message = request.Messagge,
                Recipient = request.Recipient,
                Type = request.Messagge
            };
            await _notificationService.SendNotificationAsync(notification);
        }
    }
}
