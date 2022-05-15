﻿using CarStore.AppContracts.Dtos;
using FluentValidation;
using MediatR;
using N8T.Core.Domain;

namespace ProductService.AppCore.UseCases.Queries
{
    public class GetProducts
    {
        public class Query : IQuery<IEnumerable<ProductDto>>
        {
            internal class Validator : AbstractValidator<Query>
            {
                public Validator()
                {
                }
            }

            public string? Text { get; set; }

            internal class Handler : IRequestHandler<Query, ResultModel<IEnumerable<ProductDto>>>
            {
                private readonly IRepository _repository;

                public Handler(IRepository repository)
                {
                    _repository = repository ?? throw new ArgumentNullException(nameof(repository));
                }

                public async Task<ResultModel<IEnumerable<ProductDto>>> Handle(Query request, CancellationToken cancellationToken)
                {
                    if (request == null)
                    {
                        throw new ArgumentNullException(nameof(request));
                    }

                    var products = await _repository.Get(request.Text);

                    return ResultModel<IEnumerable<ProductDto>>.Create(products);
                }
            }
        }
    }
}
