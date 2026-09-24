using System;
using System.Collections.Generic;
using System.Text;

namespace AirlineBookingSystem.Payment.Core.Entities
{
    public class Payment
    {
        public Guid Id { get; set; }    
        public Guid BookingId { get; set; }
        public int Amount { get; set; }
        public DateTime PaymentDate { get; set; }
      }
}
