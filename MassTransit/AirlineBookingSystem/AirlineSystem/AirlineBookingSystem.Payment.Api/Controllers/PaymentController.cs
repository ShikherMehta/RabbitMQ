using AirlineBookingSystem.Payment.Application.Command;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AirlineBookingSystem.Payment.Api.Controllers
{
    [ApiController]
    [Route("api/payments")]
    public class PaymentController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PaymentController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> ProcessPayment([FromBody] ProcessPaymentCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(ProcessPayment), new { id }, command);
        }
        [HttpPost("refun/{id}")]
        public async Task<IActionResult> RefundPayment(Guid Id)
        {
            await _mediator.Send(new RefundPaymentCommand(Id));
            return NoContent();
        }

    }
}
