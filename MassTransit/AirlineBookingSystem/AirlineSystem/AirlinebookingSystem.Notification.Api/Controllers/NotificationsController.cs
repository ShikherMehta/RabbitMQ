using AirlineBookingSystem.Notification.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AirlinebookingSystem.Notification.Api.Controllers
{
    [ApiController]
    [Route("api/notifications")]
    public class NotificationsController : ControllerBase
    {
       private readonly IMediator _mediator;
        public NotificationsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> SendNotification([FromBody] SendNotificationCommands commands) 
        {
            await _mediator.Send(commands);
            return Ok();

        }
    }
}
