using CarStore.AppContracts.Dtos;
using FluentValidation;
using MediatR;
using N8T.Core.Domain;

namespace ProductService.AppCore.UseCases.Queries
{
    public class GetBrands
    {
        public class Query : SearchBrandRequestDto, IQuery<IEnumerable<BrandDto>>
        {
            internal class Validator : AbstractValidator<Query>
            {
                public Validator()
                {
                }
            }

            internal class Handler : IRequestHandler<Query, ResultModel<IEnumerable<BrandDto>>>
            {
                private readonly IBrandRepository _repository;

                public Handler(IBrandRepository brandRepository)
                {
                    _repository = brandRepository ?? throw new ArgumentNullException(nameof(brandRepository));
                }

                public async Task<ResultModel<IEnumerable<BrandDto>>> Handle(Query request, CancellationToken cancellationToken)
                {
                    if (request == null)
                    {
                        throw new ArgumentNullException(nameof(request));
                    }

                    var products = await _repository.Get(request.SearchBrandModel);

                    return ResultModel<IEnumerable<BrandDto>>.Create(products);
                }
            }
        }
    }
}
