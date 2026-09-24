using AirlineBookingSystem.Payment.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AirlineBookingSystem.Payment.Core.Repositories
{
    public interface IPaymentRepository
    {
        Task ProcessPaymentAsync(AirlineBookingSystem.Payment.Core.Entities.Payment payment);
        Task RefundpaymentAsync(Guid Id);
    }
}
