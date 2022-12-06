using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.AppCore.UseCases.Commands;
using ProductService.AppCore.UseCases.Queries;
using ProductService.Shared.DTO;

namespace ProductService.Api.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    public class ProductsController : ControllerBase
    {
        private ISender? _mediator;
        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetService<ISender>();

        protected readonly string _pageSize;
        public ProductsController(IConfiguration configuration)
        {
            _pageSize = configuration.GetValue<string>("PageSize");
        }

        [HttpGet("/api/v{version:apiVersion}/products")]
        public async Task<ActionResult> HandleGetProductsAsync([FromQuery] GetProducts request, CancellationToken cancellationToken = new())
        {
            var result = await Mediator.Send(request, cancellationToken);

            return Ok(result);
        }

        [HttpGet("/api/v{version:apiVersion}/products/{id:guid}")]
        public async Task<ActionResult<ProductDto>> HandleGetProductByIdAsync(GetProductById request, CancellationToken cancellationToken = new())
        {
            return Ok(await Mediator.Send(request, cancellationToken));
        }

        [Authorize]
        [HttpPost("/api/v{version:apiVersion}/products")]
        public async Task<ActionResult> HandleCreateProductAsync([FromBody] CreateProduct request, CancellationToken cancellationToken = new())
        {
            return Ok(await Mediator.Send(request, cancellationToken));
        }
    }
}
