using System;
using System.Collections.Generic;
using System.Text;

namespace AirlineBookingSystem.Notification.Application.Interfaces
{
    public interface INotificationService
    {
        Task SendNotificationAsync(AirlineBookingSystem.Notification.Core.Entities.Notification notification);
    }
}
