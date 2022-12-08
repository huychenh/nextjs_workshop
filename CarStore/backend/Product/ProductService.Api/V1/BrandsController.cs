using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductService.AppCore.UseCases.Queries;

namespace ProductService.Api.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    public class BrandsController : ControllerBase
    {
        private ISender? _mediator;
        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetService<ISender>();
         
        [HttpGet("/api/v{version:apiVersion}/brands")]
        public async Task<ActionResult> HandleGetBrandsAsync([FromQuery] GetBrands request, CancellationToken cancellationToken = new())
        {
            var result = await Mediator.Send(request, cancellationToken);

            return Ok(result);
        }
    }
}
