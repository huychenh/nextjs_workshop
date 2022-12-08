using FluentValidation;
using MediatR;
using N8T.Core.Domain;
using ProductService.Shared.DTO;

namespace ProductService.AppCore.UseCases.Queries
{
    public class GetBrands : IQuery<IEnumerable<BrandDto>>
    {
        public string? SearchText { get; set; }

        internal class Validator : AbstractValidator<GetBrands>
        {
            public Validator()
            {
            }
        }

        internal class Handler : IRequestHandler<GetBrands, ResultModel<IEnumerable<BrandDto>>>
        {
            private readonly IBrandRepository _repository;

            public Handler(IBrandRepository brandRepository)
            {
                _repository = brandRepository ?? throw new ArgumentNullException(nameof(brandRepository));
            }

            public async Task<ResultModel<IEnumerable<BrandDto>>> Handle(GetBrands request, CancellationToken cancellationToken)
            {
                if (request == null)
                {
                    throw new ArgumentNullException(nameof(request));
                }

                var products = await _repository.Get(request);

                return ResultModel<IEnumerable<BrandDto>>.Create(products);
            }
        }
    }
}
