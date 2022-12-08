using FluentValidation;
using MediatR;
using N8T.Core.Domain;
using ProductService.Shared.DTO;

namespace ProductService.AppCore.UseCases.Queries
{
    public class GetBrandByName : IQuery<BrandDto>
    {
        public string Name { get; init; } = string.Empty;

        internal class Validator : AbstractValidator<GetBrandByName>
        {
            public Validator()
            {
                RuleFor(x => x.Name)
                    .NotNull()
                    .NotEmpty().WithMessage("Name is required.");
            }
        }

        internal class Handler : IRequestHandler<GetBrandByName, ResultModel<BrandDto>>
        {
            private readonly IBrandRepository _repository;

            public Handler(IBrandRepository brandRepository)
            {
                _repository = brandRepository ?? throw new ArgumentNullException(nameof(brandRepository));
            }

            public async Task<ResultModel<BrandDto>> Handle(GetBrandByName request, CancellationToken cancellationToken)
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
