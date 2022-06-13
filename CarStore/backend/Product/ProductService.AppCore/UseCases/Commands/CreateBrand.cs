using CarStore.AppContracts.Dtos;
using FluentValidation;
using MediatR;
using N8T.Core.Domain;
using ProductService.AppCore.Core;

namespace ProductService.AppCore.UseCases.Commands
{
    public class CreateBrand
    {
        public record Command : ICreateCommand<BrandCreateDto, Guid>
        {
            public BrandCreateDto Model { get; init; } = default!;

            internal class Validator : AbstractValidator<Command>
            {
                public Validator()
                {
                    RuleFor(v => v.Model.Name)
                        .NotEmpty().WithMessage("Name is required.")
                        .MaximumLength(20).WithMessage("Name must not exceed 20 characters.");
                }
            }

            internal class Handler : IRequestHandler<Command, ResultModel<Guid>>
            {
                private readonly IBrandRepository _repository;

                public Handler(IBrandRepository brandRepository)
                {
                    _repository = brandRepository ?? throw new ArgumentNullException(nameof(brandRepository));
                }

                public async Task<ResultModel<Guid>> Handle(Command request, CancellationToken cancellationToken)
                {
                    var brand = Brand.Create(request.Model);

                    var id = await _repository.Add(brand);

                    return ResultModel<Guid>.Create(id);
                }
            }
        }
    }
}
