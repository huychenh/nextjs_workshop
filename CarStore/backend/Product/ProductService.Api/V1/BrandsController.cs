using CarStore.AppContracts.Dtos;
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
        public async Task<ActionResult> HandleGetBrandsAsync([FromQuery] SearchBrandDto request, CancellationToken cancellationToken = new())
        {
            var queryModel = new GetBrands.Query { SearchBrandModel = request };

            var result = await Mediator.Send(queryModel, cancellationToken);

            return Ok(result);
        }
    }
}
