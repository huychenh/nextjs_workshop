﻿using CarStore.AppContracts.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using N8T.Core.Domain;
using N8T.Infrastructure;
using ProductService.AppCore.UseCases.Commands;
using ProductService.AppCore.UseCases.Queries;

namespace ProductService.Api.V2
{
    [ApiController]
    [ApiVersion("2.0")]
    public class ProductsController : ControllerBase
    {
        private ISender? _mediator;
        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetService<ISender>();

        [MapToApiVersion("2.0")]
        [HttpGet("/api/v{version:apiVersion}/products")]
        public async Task<ActionResult> HandleGetProductsAsync(
            string query,
            CancellationToken cancellationToken = new())
        {
            var queryModel = HttpContext.SafeGetListQuery<GetProducts.Query, ListResultModel<ProductDto>>(query);

            return Ok(await Mediator.Send(queryModel, cancellationToken));
        }

        [MapToApiVersion("2.0")]
        [HttpGet("/api/v{version:apiVersion}/products/{id:guid}")]
        public async Task<ActionResult<ProductDto>> HandleGetProductByIdAsync(
            Guid id,
            CancellationToken cancellationToken = new())
        {
            var request = new GetProductById.Query { Id = id };

            return Ok(await Mediator.Send(request, cancellationToken));
        }

        [MapToApiVersion("2.0")]
        [HttpPost("/api/v{version:apiVersion}/products")]
        public async Task<ActionResult> HandleCreateProductAsync(
            [FromBody] CreateProduct.Command request,
            CancellationToken cancellationToken = new())
        {
            return Ok(await Mediator.Send(request, cancellationToken));
        }
    }
}