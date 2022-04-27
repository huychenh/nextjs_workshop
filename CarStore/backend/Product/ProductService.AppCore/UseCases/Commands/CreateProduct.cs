using CarStore.AppContracts.Dtos;
using FluentValidation;
using MediatR;
using N8T.Core.Domain;
using ProductService.AppCore.Core;

namespace ProductService.AppCore.UseCases.Commands
{
    public class CreateProduct
    {
        public record Command : ICreateCommand<Command.CreateProductModel, Guid>
        {
            public CreateProductModel Model { get; init; } = default!;

            public record CreateProductModel(string Name, int Quantity, decimal Cost, string ProductCodeName);

            internal class Validator : AbstractValidator<Command>
            {
                public Validator()
                {
                    RuleFor(v => v.Model.Name)
                        .NotEmpty().WithMessage("Name is required.")
                        .MaximumLength(50).WithMessage("Name must not exceed 50 characters.");

                    RuleFor(v => v.Model.ProductCodeName)
                        .NotEmpty().WithMessage("ProductCodeName is required.")
                        .MaximumLength(5).WithMessage("ProductCodeName must not exceed 5 characters.");

                    RuleFor(x => x.Model.Quantity)
                        .GreaterThanOrEqualTo(1).WithMessage("Quantity should be at least greater than or equal to 1.");

                    RuleFor(x => x.Model.Cost)
                        .GreaterThanOrEqualTo(1000).WithMessage("Cost should be greater than 1000.");
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
                    var product = Product.Create(request.Model.Name, request.Model.Quantity,request.Model.Cost);
                    var id = await _repository.Add(product);

                    return ResultModel<Guid>.Create(id);
                }
            }
        }
    }
}
