using AirlineBookingSystem.BuildingBlocks.Contracts;
using AirlineBookingSystem.Notification.Application.Interfaces;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;

namespace AirlineBookingSystem.Notification.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IPublishEndpoint _publishEndPoint;
        public NotificationService(IPublishEndpoint publishEndpoint) 
        {
            _publishEndPoint = publishEndpoint;
        }
        public async Task SendNotificationAsync(AirlineBookingSystem.Notification.Core.Entities.Notification notification)
        {
            Console.WriteLine($"Notification sent to {notification.Recipient} : {notification.Message}");

            //Publish the event

            var notificationEvent = new NotificationEvent(notification.Recipient, notification.Message, notification.Type);
            await _publishEndPoint.Publish(notificationEvent);
        }
    }
}
