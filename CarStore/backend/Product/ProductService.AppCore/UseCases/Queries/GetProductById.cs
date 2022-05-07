using CarStore.AppContracts.Dtos;
using FluentValidation;
using MediatR;
using N8T.Core.Domain;

namespace ProductService.AppCore.UseCases.Queries
{
    public class GetProductById
    {
        public record Query : IQuery<ProductDto>
        {
            public Guid Id { get; init; }

            internal class Validator : AbstractValidator<Query>
            {
                public Validator()
                {
                    RuleFor(x => x.Id)
                        .NotNull()
                        .NotEmpty().WithMessage("Id is required.");
                }
            }

            internal class Handler : IRequestHandler<Query, ResultModel<ProductDto>>
            {
                private readonly IRepository _repository;

                public Handler(IRepository productRepository)
                {
                    _repository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
                }

                public async Task<ResultModel<ProductDto>> Handle(Query request, CancellationToken cancellationToken)
                {
                    if (request == null)
                    {
                        throw new ArgumentNullException(nameof(request));
                    }

                    var product = await _repository.GetById(request.Id);

                    return ResultModel<ProductDto>.Create(product);
                }
            }
        }
    }
}
