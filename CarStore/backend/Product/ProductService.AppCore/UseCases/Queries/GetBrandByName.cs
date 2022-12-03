using FluentValidation;
using MediatR;
using N8T.Core.Domain;
using ProductService.Shared.DTO;

namespace ProductService.AppCore.UseCases.Queries
{
    public class GetBrandByName
    {
        public record Query : IQuery<BrandDto>
        {
            public string? Name { get; init; }

            internal class Validator : AbstractValidator<Query>
            {
                public Validator()
                {
                    RuleFor(x => x.Name)
                        .NotNull()
                        .NotEmpty().WithMessage("Name is required.");
                }
            }

            internal class Handler : IRequestHandler<Query, ResultModel<BrandDto>>
            {
                private readonly IBrandRepository _repository;

                public Handler(IBrandRepository brandRepository)
                {
                    _repository = brandRepository ?? throw new ArgumentNullException(nameof(brandRepository));
                }

                public async Task<ResultModel<BrandDto>> Handle(Query request, CancellationToken cancellationToken)
                {
                    if (request == null)
                    {
                        throw new ArgumentNullException(nameof(request));
                    }

                    var brand = await _repository.GetByName(request.Name);

                    return ResultModel<BrandDto>.Create(brand);
                }
            }
        }
    }
}
