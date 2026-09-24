using System;

public record PaymentProcessEvent(Guid PaymentId, Guid BookingId, decimal Amount, DateTime PaymentDate);
