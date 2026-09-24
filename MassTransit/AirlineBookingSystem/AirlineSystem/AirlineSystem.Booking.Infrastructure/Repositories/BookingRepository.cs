using AirlineBookingSystem.Booking.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Microsoft.Data.SqlClient;
using Dapper;
namespace AirlineSystem.Booking.Infrastructure.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly IDbConnection _dbConnection;

        public BookingRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        public async Task AddBookingAsync(AirlineBookingSystem.Booking.Core.Entities.Booking booking)
        {
            const string sql = @"Insert into bookings (Id,FlightId,PassengerName,SeatNumber,BookingDate) 
                                 values(@Id,@FlightId,@PassengerName,@SeatNumber,@BookingDate)";
            await _dbConnection.ExecuteAsync(sql, booking);
        }
        public async Task<AirlineBookingSystem.Booking.Core.Entities.Booking> GetBookingByIdAsync(Guid Id)
        {
            const string sql = @"SELECT * FROM Bookings WHERE Id = @Id";
            return await _dbConnection.QuerySingleOrDefaultAsync<AirlineBookingSystem.Booking.Core.Entities.Booking>(sql, new { Id = Id });
        }
    }
}
