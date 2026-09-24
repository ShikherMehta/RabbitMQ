using AirlineBookingSystem.Booking.Core.Repositories;
using AirlineBookingSystem.BuildingBlocks.Contracts;
using MassTransit;
using MediatR;

namespace AirlineBookingSystem.Booking.Application.Commands
{
    public class CreateBookingHandler : IRequestHandler<CreateBookingCommand,Guid>
    { 
        private readonly IBookingRepository _repository;
        private readonly IPublishEndpoint _publishEndPoint;
        public CreateBookingHandler(IBookingRepository repository, IPublishEndpoint publishEndpoint) 
        {
            _repository = repository;
            _publishEndPoint = publishEndpoint;
        }
        public async Task<Guid> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            var booking = new AirlineBookingSystem.Booking.Core.Entities.Booking
            {
                Id = Guid.NewGuid(),
                FlightId = request.FlightId,
                PassengerName = request.passengerName,
                SeatNumber = request.SeatNumber,
                BookingDate = DateTime.UtcNow
            };

            await _repository.AddBookingAsync(booking);

            //Publish Flight Booked Event
            Console.WriteLine("Before Publish");
            await _publishEndPoint.Publish(new FlightBookedEvent(
                booking.Id,
                booking.FlightId,
                booking.PassengerName,
                booking.SeatNumber,
                booking.BookingDate
                ));
            Console.WriteLine("After Publish");
            return booking.Id;
        }
    }
}
