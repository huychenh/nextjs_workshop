using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderingService.AppCore.UseCases.Commands;
using OrderingService.AppCore.UseCases.Queries;
using OrderingService.Shared.DTO;

namespace OrderingService.Api.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    public class OrdersController : ControllerBase
    {
        private ISender? _mediator;
        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetService<ISender>();

        [Authorize]
        [HttpPost("/api/v{version:apiVersion}/orders")]
        public async Task<ActionResult> HandleCreateOrderAsync([FromBody] CreateOrder request, CancellationToken cancellationToken = new())
        {
            return Ok(await Mediator.Send(request, cancellationToken));
        }

        [Authorize]
        [HttpGet("/api/v{version:apiVersion}/orders/")]
        public async Task<ActionResult<OrderDto>> HandleGetOrdersByCustomerIdAsync(CancellationToken cancellationToken = new())
        {
            var request = new GetOrders();

            return Ok(await Mediator.Send(request, cancellationToken));
        }
    }
}
