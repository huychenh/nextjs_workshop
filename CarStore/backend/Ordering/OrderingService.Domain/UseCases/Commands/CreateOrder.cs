using CarStore.AppContracts.Dtos;
using FluentValidation;
using MediatR;
using N8T.Core.Domain;

namespace OrderingService.AppCore.UseCases.Commands
{
    public class CreateOrder
    {
        public record Command : ICreateCommand<CreateOrderDto, Guid>
        {
            public CreateOrderDto Model { get; init; } = default!;

            internal class Validator : AbstractValidator<Command>
            {
                public Validator()
                {
                    RuleFor(v => v.Model.ProductId)
                        .NotEmpty();

                    RuleFor(v => v.Model.Price)
                        .NotEmpty();

                    RuleFor(v => v.Model.OwnerId)
                        .NotEmpty();

                    RuleFor(v => v.Model.BuyerId)
                        .NotEmpty();
                }
            }

            internal class Handler : IRequestHandler<Command, ResultModel<Guid>>
            {
                public async Task<ResultModel<Guid>> Handle(Command request, CancellationToken cancellationToken)
                {
                    return null;
                }
            }
        }
    }
}
