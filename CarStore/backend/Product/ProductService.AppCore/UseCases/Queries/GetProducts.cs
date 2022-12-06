using FluentValidation;
using MediatR;
using N8T.Core.Domain;
using ProductService.AppCore.Services;
using ProductService.Shared.DTO;

namespace ProductService.AppCore.UseCases.Queries
{
    public class GetProducts : IQuery<ListResultModel<ProductDto>>
    {
        public string? SearchText { get; set; }

        public string? CategoryName { get; set; }

        public string? Brand { get; set; }

        public decimal PriceFrom { get; set; }

        public decimal PriceTo { get; set; }

        public bool? LatestNews { get; set; }

        public bool? LowestPrice { get; set; }

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        internal class Validator : AbstractValidator<GetProducts>
        {
            public Validator()
            {
            }
        }

        internal class Handler : IRequestHandler<GetProducts, ResultModel<ListResultModel<ProductDto>>>
        {
            private readonly IRepository _repository;
            private readonly IFileStorageService _fileStorageService;

            public Handler(IRepository repository, IFileStorageService fileStorageService)
            {
                _repository = repository ?? throw new ArgumentNullException(nameof(repository));
                _fileStorageService = fileStorageService;
            }

            public async Task<ResultModel<ListResultModel<ProductDto>>> Handle(GetProducts request, CancellationToken cancellationToken)
            {
                if (request == null)
                {
                    throw new ArgumentNullException(nameof(request));
                }

                var result = await _repository.GetWithPagination(request);

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
