﻿using CarStore.AppContracts.Dtos;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using N8T.Core.Domain;
using OrderingService.AppCore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingService.AppCore.UseCases.Queries
{
    public class GetOrders
    {
        public class Query : IQuery<IEnumerable<OrderDto>>
        {
            internal class Validator : AbstractValidator<Query>
            {
                public Validator()
                {
                }
            }

            internal class Handler : IRequestHandler<Query, ResultModel<IEnumerable<OrderDto>>>
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

                public async Task<ResultModel<IEnumerable<OrderDto>>> Handle(Query request, CancellationToken cancellationToken)
                {
                    var buyerId = GetCurrentUserId();
                    IEnumerable<OrderDto> orders;
                    orders = await _repository.GetOrdersByCustomerId(buyerId);
                    return ResultModel<IEnumerable<OrderDto>>.Create(orders);
                }

                private Guid GetCurrentUserId()
                {
                    var claim = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "sub");
                    if (claim == null)
                    {
                        throw new UnauthorizedAccessException("Please sign in first.");
                    }

                    return Guid.Parse(claim.Value);
                }
            }
        }
    }
}