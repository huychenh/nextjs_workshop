using CarStore.AppContracts.Dtos;
using FluentValidation;
using MediatR;
using N8T.Core.Domain;
using ProductService.AppCore.Core;

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

                    RuleFor(x => x.Model.OwnerId)
                        .NotEmpty().WithMessage("OwnerId is required.");
                }
            }

            internal class Handler : IRequestHandler<Command, ResultModel<Guid>>
            {
                private readonly IRepository _repository;

                public Handler(IRepository productRepository)
                {
                    _repository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
                }

                public async Task<ResultModel<Guid>> Handle(Command request, CancellationToken cancellationToken)
                {
                    var product = Product.Create(request.Model);

                    var id = await _repository.Add(product);

                    return ResultModel<Guid>.Create(id);
                }
            }
        }
    }
}
