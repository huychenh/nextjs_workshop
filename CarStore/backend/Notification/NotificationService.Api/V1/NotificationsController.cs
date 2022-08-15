using MediatR;
using Microsoft.AspNetCore.Mvc;
using NotificationService.AppCore.UseCases.Commands;

namespace NotificationService.Api.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    public class NotificationsController : ControllerBase
    {
        private ISender? _mediator;
        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetService<ISender>();

        [HttpPost("/api/v{version:apiVersion}/notification")]
        public async Task<ActionResult> HandleSendNotificationAsync([FromBody] SendEmail.Command request
            , CancellationToken cancellationToken = new())
        {
            return Ok(await Mediator.Send(request, cancellationToken));
        }
    }
}
