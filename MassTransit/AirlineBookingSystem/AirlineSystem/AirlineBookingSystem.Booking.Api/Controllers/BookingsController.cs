using AirlineBookingSystem.Booking.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using AirlineBookingSystem.Booking.Application.Queries;

namespace AirlineBookingSystem.Booking.Api.Controllers
{
    [ApiController]
    [Route("api/bookings")]
    public class BookingsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public BookingsController(IMediator mediator) 
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> AddBooking([FromBody] CreateBookingCommand command)
        { 
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetBookingById), new { id }, command);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById(Guid Id)
        {
            var booking = await _mediator.Send(new GetBookingQuery(Id));
            return Ok(booking);
        }
    }
}
