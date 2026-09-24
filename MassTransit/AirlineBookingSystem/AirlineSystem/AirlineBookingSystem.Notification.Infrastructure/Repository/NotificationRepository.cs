using AirlineBookingSystem.Notification.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Dapper;
namespace AirlineBookingSystem.Notification.Infrastructure.Repository
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly IDbConnection _dbConnection;
        public NotificationRepository(IDbConnection dbConnection) 
        {
            _dbConnection = dbConnection;
        }
        public async Task LogNotificationAsync(Core.Entities.Notification notification)
        {
            const string sql = @"Insert Into Notifications(Id,Recipient,Message,Type,SendAt) 
                               Values(@Id,@Recipient,@Message,@Type,@SendAt)";

            await _dbConnection.ExecuteAsync(sql, notification);
        }
    }
}
