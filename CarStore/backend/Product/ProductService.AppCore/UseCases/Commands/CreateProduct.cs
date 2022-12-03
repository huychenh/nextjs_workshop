using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using N8T.Core.Domain;
using ProductService.AppCore.Core;
using ProductService.Shared.DTO;

namespace ProductService.AppCore.UseCases.Commands
{
    public class CreateProduct
    {
        public record Command : ICreateCommand<ProductCreateDto, Guid>
        {
            public ProductCreateDto Model { get; init; } = default!;

            internal class Validator : AbstractValidator<Command>
            {
                public Validator()
                {
                    RuleFor(v => v.Model.Name)
                        .NotEmpty().WithMessage("Name is required.")
                        .MaximumLength(50).WithMessage("Name must not exceed 50 characters.");

                    RuleFor(x => x.Model.Price)
                        .GreaterThan(0).WithMessage("Price should be greater than 0.");

                    RuleFor(v => v.Model.Brand)
                        .NotEmpty().WithMessage("Brand is required.");

                    RuleFor(x => x.Model.Model)
                        .NotEmpty().WithMessage("Model is required.");

                    RuleFor(x => x.Model.Transmission)
                        .IsEnumName(typeof(Transmission));

                    RuleFor(x => x.Model.FuelType)
                        .IsEnumName(typeof(FuelType));
                }
            }

            internal class Handler : IRequestHandler<Command, ResultModel<Guid>>
            {
                private readonly IRepository _repository;
                private readonly IBrandRepository _brandRepository;
                private readonly IHttpContextAccessor _httpContextAccessor;

                public Handler(IRepository productRepository, IBrandRepository brandRepository, IHttpContextAccessor httpContextAccessor)
                {
                    _repository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
                    _brandRepository = brandRepository ?? throw new ArgumentNullException(nameof(productRepository));
                    _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
                }

                public async Task<ResultModel<Guid>> Handle(Command request, CancellationToken cancellationToken)
                {
                    var brandDto = await _brandRepository.GetByName(request.Model.Brand);

                    var brandId = brandDto?.Id ?? Guid.Empty; 

                    if (brandId == Guid.Empty)
                    {
                        var brand = new Brand { Name = request.Model.Brand };
                        brandId = await _brandRepository.Add(brand);
                    }

                    var userId = GetCurrentUserId();
                    if (userId == null)
                    {
                        throw new UnauthorizedAccessException("Please sign in first.");
                    }

                    var product = Product.Create(request.Model, brandId, userId.Value);

                    var id = await _repository.Add(product);

                    return ResultModel<Guid>.Create(id);
                }

                private Guid? GetCurrentUserId()
                {
                    var idClaim = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "sub");
                    if (idClaim == null || !Guid.TryParse(idClaim.Value, out Guid id))
                    {
                        return null;
                    }

                    return id;
                }
            }
        }
    }
}
