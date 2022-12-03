using FluentValidation;
using MediatR;
using N8T.Core.Domain;
using ProductService.AppCore.Services;
using ProductService.Shared.DTO;

namespace ProductService.AppCore.UseCases.Queries
{
    public class GetProducts
    {
        public class Query : SearchProductRequestDto, IQuery<ListResultModel<ProductDto>>
        {
            internal class Validator : AbstractValidator<Query>
            {
                public Validator()
                {
                }
            }

            internal class Handler : IRequestHandler<Query, ResultModel<ListResultModel<ProductDto>>>
            {
                private readonly IRepository _repository;
                private readonly IFileStorageService _fileStorageService;

                public Handler(IRepository repository, IFileStorageService fileStorageService)
                {
                    _repository = repository ?? throw new ArgumentNullException(nameof(repository));
                    _fileStorageService = fileStorageService;
                }

                public async Task<ResultModel<ListResultModel<ProductDto>>> Handle(Query request, CancellationToken cancellationToken)
                {
                    if (request == null)
                    {
                        throw new ArgumentNullException(nameof(request));
                    }

                    var result = await _repository.GetWithPagination(request.SearchProductModel);

                    foreach (var product in result.Items)
                    {
                        if (product.Images == null)
                        {
                            continue;
                        }

                        for (int i = 0; i < product.Images.Count(); i++)
                        {
                            product.Images[i] = _fileStorageService.BuildFileUrl(product.Images[i]);
                        }
                    }
                    
                    return ResultModel<ListResultModel<ProductDto>>.Create(result);
                }
            }
        }
    }
}
