using System;
using System.Collections.Generic;
using System.Text;

namespace AirlineBookingSystem.Notification.Core.Repositories
{
    public interface INotificationRepository
    {
        Task LogNotificationAsync(AirlineBookingSystem.Notification.Core.Entities.Notification notification);
    }
}
