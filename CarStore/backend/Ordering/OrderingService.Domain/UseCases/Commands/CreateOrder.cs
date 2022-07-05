using CarStore.AppContracts.Dtos;
using FluentValidation;
using MediatR;
using N8T.Core.Domain;
using OrderingService.AppCore.Core;

namespace OrderingService.AppCore.UseCases.Commands
{
    public class CreateOrder
    {
        public record Command : ICreateCommand<CreateOrderDto, Guid>
        {
            public CreateOrderDto Model { get; init; } = default!;
            public string buyerEmail { get; init; } = default!;
            public string ownerEmail { get; init; } = default!;

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
                private readonly IOrderRepository _repository;

                public Handler(IOrderRepository orderRepository)
                {
                    _repository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
                }
                public async Task<ResultModel<Guid>> Handle(Command request, CancellationToken cancellationToken)
                {
                    var order = Order.Create(request.Model, request.buyerEmail, request.ownerEmail);

                    var id = await _repository.Add(order);

                    return ResultModel<Guid>.Create(id);
                }
            }
        }
    }
}
