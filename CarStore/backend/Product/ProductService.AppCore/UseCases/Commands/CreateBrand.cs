using FluentValidation;
using MediatR;
using N8T.Core.Domain;
using ProductService.AppCore.Core;

namespace ProductService.AppCore.UseCases.Commands
{
    public class CreateBrand : ICommand<Guid>
    {
        public string Name { get; set; } = string.Empty;

        internal class Validator : AbstractValidator<CreateBrand>
        {
            public Validator()
            {
                RuleFor(v => v.Name)
                    .NotEmpty().WithMessage("Name is required.")
                    .MaximumLength(20).WithMessage("Name must not exceed 20 characters.");
            }
        }

        internal class Handler : IRequestHandler<CreateBrand, ResultModel<Guid>>
        {
            private readonly IBrandRepository _repository;

            public Handler(IBrandRepository brandRepository)
            {
                _repository = brandRepository ?? throw new ArgumentNullException(nameof(brandRepository));
            }

            public async Task<ResultModel<Guid>> Handle(CreateBrand request, CancellationToken cancellationToken)
            {
                var brand = Brand.Create(request.Name);

                var id = await _repository.Add(brand);

                return ResultModel<Guid>.Create(id);
            }
        }
    }
}
