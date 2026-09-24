using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using MediatR;
namespace AirlineBookingSystem.Booking.Application.Queries
{
    public record GetBookingQuery(Guid Id) : IRequest<AirlineBookingSystem.Booking.Core.Entities.Booking>;
    
}
