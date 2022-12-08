using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using N8T.Core.Domain;
using OrderingService.AppCore.Services;
using OrderingService.Shared.DTO;

namespace OrderingService.AppCore.UseCases.Queries
{
    public class GetOrders : IQuery<IEnumerable<OrderDto>>
    {
        internal class Validator : AbstractValidator<GetOrders>
        {
            public Validator()
            {
            }
        }

        internal class Handler : IRequestHandler<GetOrders, ResultModel<IEnumerable<OrderDto>>>
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

            public async Task<ResultModel<IEnumerable<OrderDto>>> Handle(GetOrders request, CancellationToken cancellationToken)
            {
                var buyerId = GetCurrentUserId();
                IEnumerable<OrderDto> orders;
                orders = await _repository.GetOrdersByCustomerId(buyerId);
                return ResultModel<IEnumerable<OrderDto>>.Create(orders);
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
