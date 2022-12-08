using FluentValidation;
using MediatR;
using N8T.Core.Domain;
using ProductService.AppCore.Services;
using ProductService.Shared.DTO;

namespace ProductService.AppCore.UseCases.Queries
{
    public class GetProductById : IQuery<ProductDto>
    {
        public Guid Id { get; init; }

        internal class Validator : AbstractValidator<GetProductById>
        {
            public Validator()
            {
                RuleFor(x => x.Id)
                    .NotNull()
                    .NotEmpty().WithMessage("Id is required.");
            }
        }

        internal class Handler : IRequestHandler<GetProductById, ResultModel<ProductDto>>
        {
            private readonly IRepository _repository;
            private readonly IFileStorageService _fileStorageService;

            public Handler(IRepository productRepository, IFileStorageService fileStorageService)
            {
                _repository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
                _fileStorageService = fileStorageService;
            }

            public async Task<ResultModel<ProductDto>> Handle(GetProductById request, CancellationToken cancellationToken)
            {
                if (request == null)
                {
                    throw new ArgumentNullException(nameof(request));
                }

                var product = await _repository.GetById(request.Id);

                if (product != null && product.Images != null)
                {
                    for (int i = 0; i < product.Images.Length; i++)
                    {
                        product.Images[i] = _fileStorageService.BuildFileUrl(product.Images[i]);
                    }
                }

                return ResultModel<ProductDto>.Create(product);
            }
        }
    }
}
