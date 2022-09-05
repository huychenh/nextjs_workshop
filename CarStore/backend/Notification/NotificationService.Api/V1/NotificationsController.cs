using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotificationService.AppCore.UseCases.Commands;

namespace OrderingService.Api.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    public class NotificationsController : ControllerBase
    {
        private ISender? _mediator;
        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetService<ISender>();

        protected readonly string _fromEmail;
        protected readonly string _fromName;

        public NotificationsController(IConfiguration configuration)
        {
            _fromEmail = configuration.GetSection("SendGridEmailSettings")
                                      .GetValue<string>("FromEmail");
            _fromName = configuration.GetSection("SendGridEmailSettings")
                                      .GetValue<string>("FromName");
        }

        [HttpPost("/api/v{version:apiVersion}/notifications")]
        public async Task<ActionResult> HandleCreateOrderAsync([FromBody] SendNotification.Command request, CancellationToken cancellationToken = new())
        {
            request.Model.From = _fromEmail;
            request.Model.FromName = _fromName;
            return Ok(await Mediator.Send(request, cancellationToken));
        }
    }
}
