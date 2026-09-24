using AirlineBookingSystem.Payment.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Dapper;
namespace AirlineBookingSystem.Payment.Infrastructure.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly IDbConnection _dbConnection;
        public PaymentRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        public async Task ProcessPaymentAsync(Core.Entities.Payment payment)
        {
            const string sql = @"Insert Into Payments(Id,BookingId,Amount,PaymentDate) 
                               values(@Id,@BookingId,@Amount,@PaymentDate)";
            await _dbConnection.ExecuteAsync(sql, payment);
        }

        public async Task RefundpaymentAsync(Guid Id)
        {
            const string sql = @"DELETE FROM Payments WHERE Id=@Id";
            await _dbConnection.ExecuteAsync(sql, new { Id = Id });
        }
    }
}
