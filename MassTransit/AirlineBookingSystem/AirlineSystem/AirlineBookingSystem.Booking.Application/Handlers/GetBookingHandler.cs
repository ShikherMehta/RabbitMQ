using AirlineBookingSystem.Booking.Application.Queries;
using AirlineBookingSystem.Booking.Core.Repositories;
using MediatR;
using MassTransit;
namespace AirlineBookingSystem.Booking.Application.Handlers
{
    public class GetBookingHandler : IRequestHandler<GetBookingQuery, AirlineBookingSystem.Booking.Core.Entities.Booking>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IPublishEndpoint _publishEndPoint;
        public GetBookingHandler(IBookingRepository bookingRepository, IPublishEndpoint publishEndpoint) 
        {
            _bookingRepository = bookingRepository;
            _publishEndPoint = publishEndpoint;
        }
        public async Task<AirlineBookingSystem.Booking.Core.Entities.Booking> Handle(GetBookingQuery request, CancellationToken cancellationToken)
        {
            return await _bookingRepository.GetBookingByIdAsync(request.Id);
        }
    }
}
