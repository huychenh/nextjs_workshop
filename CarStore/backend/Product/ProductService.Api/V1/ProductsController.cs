using CarStore.AppContracts.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.AppCore.UseCases.Commands;
using ProductService.AppCore.UseCases.Queries;

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
        public async Task<ActionResult> HandleGetProductsAsync([FromQuery] SearchProductDto request, CancellationToken cancellationToken = new())
        {
            if(request != null && request.Page != null && request.PageSize == null)
            {
                request.PageSize = Convert.ToInt32(_pageSize);
            }

            GetProducts.Query queryModel = new GetProducts.Query { SearchProductModel = request };

            var result = await Mediator.Send(queryModel, cancellationToken);

            return Ok(result);
        }

        [HttpGet("/api/v{version:apiVersion}/products/{id:guid}")]
        public async Task<ActionResult<ProductDto>> HandleGetProductByIdAsync(Guid id, CancellationToken cancellationToken = new())
        {
            var request = new GetProductById.Query { Id = id };

            return Ok(await Mediator.Send(request, cancellationToken));
        }

        [Authorize]
        [HttpPost("/api/v{version:apiVersion}/products")]
        public async Task<ActionResult> HandleCreateProductAsync([FromBody] CreateProduct.Command request, CancellationToken cancellationToken = new())
        {
            return Ok(await Mediator.Send(request, cancellationToken));
        }
    }
}
