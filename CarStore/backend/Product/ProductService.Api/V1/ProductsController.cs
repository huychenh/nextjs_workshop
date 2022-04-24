﻿using CarStore.AppContracts.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using N8T.Core.Domain;
using N8T.Infrastructure;
using ProductService.AppCore.UseCases.Commands;
using ProductService.AppCore.UseCases.Queries;

namespace ProductService.Api.V1
{
    [AllowAnonymous]
    [ApiController]
    [ApiVersion("1.0")]
    public class ProductsController : ControllerBase
    {
        private ISender? _mediator;
        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetService<ISender>();

        [HttpGet("/api/v{version:apiVersion}/products2")]
        public async Task<ActionResult> HandleGetProductsAsync(
            [FromQuery] string? name,
            [FromQuery] string[] sorts,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            CancellationToken cancellationToken = new())
        {
            var filters = !string.IsNullOrEmpty(name)
                ? new List<FilterModel>
                {
                    new FilterModel("Name", "Contains", name),
                }
                : new List<FilterModel>();

            var queryModel = new GetProducts.Query
            {
                Filters = filters,
                Sorts = sorts.ToList(),
                Page = page,
                PageSize = pageSize,
            };

            return Ok(await Mediator.Send(queryModel, cancellationToken));
        }

        [HttpGet("/api/v{version:apiVersion}/products")]
        public async Task<ActionResult> HandleGetProductsAsync(string query, CancellationToken cancellationToken = new())
        {
            var queryModel = HttpContext.SafeGetListQuery<GetProducts.Query, ListResultModel<ProductDto>>(query);

            return Ok(await Mediator.Send(queryModel, cancellationToken));
        }

        [HttpGet("/api/v{version:apiVersion}/products/{id:guid}")]
        public async Task<ActionResult<ProductDto>> HandleGetProductByIdAsync(Guid id, CancellationToken cancellationToken = new())
        {
            var request = new GetProductById.Query { Id = id };

            return Ok(await Mediator.Send(request, cancellationToken));
        }

        [HttpPost("/api/v{version:apiVersion}/products")]
        public async Task<ActionResult> HandleCreateProductAsync([FromBody] CreateProduct.Command request, CancellationToken cancellationToken = new())
        {
            return Ok(await Mediator.Send(request, cancellationToken));
        }
    }
}