using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using N8T.Core.Domain;
using OrderingService.AppCore.Core;
using OrderingService.AppCore.Services;
using OrderingService.Shared.DTO;

namespace OrderingService.AppCore.UseCases.Commands
{
    public class CreateOrder : ICreateCommand<CreateOrderDto, Guid>
    {
        public CreateOrderDto Model { get; init; } = default!;

        internal class Validator : AbstractValidator<CreateOrder>
        {
            public Validator()
            {
                RuleFor(v => v.Model.ProductId)
                    .NotEmpty();

                RuleFor(v => v.Model.Price)
                    .NotEmpty();

                RuleFor(v => v.Model.OwnerId)
                    .NotEmpty();
            }
        }

        internal class Handler : IRequestHandler<CreateOrder, ResultModel<Guid>>
        {
            private readonly IOrderRepository _repository;
            private readonly UserInfoService _userInfoService;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public Handler(IOrderRepository orderRepository, UserInfoService userInfoService, IHttpContextAccessor httpContextAccessor)
            {
                _repository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
                _userInfoService = userInfoService ?? throw new ArgumentNullException(nameof(userInfoService));
                _httpContextAccessor = httpContextAccessor;
            }

            public async Task<ResultModel<Guid>> Handle(CreateOrder request, CancellationToken cancellationToken)
            {
                string? ownerEmail;
                try
                {
                    ownerEmail = await _userInfoService.GetUserEmail(request.Model.OwnerId);
                }
                catch
                {
                    // Ignore failure connection
                    ownerEmail = null;
                }

                var buyerId = GetCurrentUserId();

                var order = Order.Create(request.Model, buyerId, ownerEmail);

                var id = await _repository.Add(order);

                return ResultModel<Guid>.Create(id);
            }

            private Guid GetCurrentUserId()
            {
                var claim = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == "sub");
                if (claim == null)
                {
                    throw new UnauthorizedAccessException("Please sign in first.");
                }

                return Guid.Parse(claim.Value);
            }
        }
    }
}
