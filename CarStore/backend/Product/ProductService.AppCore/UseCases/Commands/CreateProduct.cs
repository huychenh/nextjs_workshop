using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using N8T.Core.Domain;
using ProductService.AppCore.Core;

namespace ProductService.AppCore.UseCases.Commands
{
    public class CreateProduct : ICommand<Guid>
    {
        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public string Brand { get; set; } = string.Empty;

        public string Model { get; set; } = string.Empty;

        public string Transmission { get; set; } = string.Empty;

        public string? MadeIn { get; set; }

        public int SeatingCapacity { get; set; }

        public int KmDriven { get; set; }

        public int Year { get; set; }

        public string FuelType { get; set; } = string.Empty;

        public string? Category { get; set; }

        public string? Color { get; set; }

        public string? Description { get; set; }

        public bool HasInstallment { get; set; }

        public ICollection<string> Images { get; set; } = new List<string>();

        internal class Validator : AbstractValidator<CreateProduct>
        {
            public Validator()
            {
                RuleFor(v => v.Name)
                    .NotEmpty().WithMessage("Name is required.")
                    .MaximumLength(50).WithMessage("Name must not exceed 50 characters.");

                RuleFor(x => x.Price)
                    .GreaterThan(0).WithMessage("Price should be greater than 0.");

                RuleFor(v => v.Brand)
                    .NotEmpty().WithMessage("Brand is required.");

                RuleFor(x => x.Model)
                    .NotEmpty().WithMessage("Model is required.");

                RuleFor(x => x.Transmission)
                    .IsEnumName(typeof(Transmission));

                RuleFor(x => x.FuelType)
                    .IsEnumName(typeof(FuelType));
            }
        }

        internal class Handler : IRequestHandler<CreateProduct, ResultModel<Guid>>
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

            public async Task<ResultModel<Guid>> Handle(CreateProduct request, CancellationToken cancellationToken)
            {
                var brandDto = await _brandRepository.GetByName(request.Brand);

                var brandId = brandDto?.Id ?? Guid.Empty;

                if (brandId == Guid.Empty)
                {
                    var brand = new Brand { Name = request.Brand };
                    brandId = await _brandRepository.Add(brand);
                }

                var userId = GetCurrentUserId();
                if (userId == null)
                {
                    throw new UnauthorizedAccessException("Please sign in first.");
                }

                var product = Product.Create(
                    request.Name,
                    request.Price,
                    request.Model,
                    (Transmission)Enum.Parse(typeof(Transmission), request.Transmission),
                    request.MadeIn,
                    request.SeatingCapacity,
                    request.KmDriven,
                    request.Year,
                    (FuelType)Enum.Parse(typeof(FuelType), request.FuelType),
                    request.Category,
                    request.Color,
                    request.Description,
                    request.HasInstallment,
                    request.Images,
                    brandId,
                    userId.Value);

                var id = await _repository.Add(product);

                return ResultModel<Guid>.Create(id);
            }

            private Guid? GetCurrentUserId()
            {
                var context = _httpContextAccessor.HttpContext;
                if (context == null)
                {
                    return null;
                }

                var idClaim = context.User.Claims.FirstOrDefault(x => x.Type == "sub");
                if (idClaim == null || !Guid.TryParse(idClaim.Value, out Guid id))
                {
                    return null;
                }

                return id;
            }
        }
    }
}
