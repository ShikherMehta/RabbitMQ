using AirlineBookingSystem.Booking.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AirlineBookingSystem.Booking.Core.Repositories
{
    public interface IBookingRepository
    {
        Task<AirlineBookingSystem.Booking.Core.Entities.Booking> GetBookingByIdAsync(Guid Id);
        Task AddBookingAsync(AirlineBookingSystem.Booking.Core.Entities.Booking booking);
    }
}
