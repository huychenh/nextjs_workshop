using CarStore.AppContracts.Dtos;
using CarStore.IntegrationEvents.Notification;
using FluentValidation;
using MediatR;
using N8T.Core.Domain;
using OrderingService.AppCore.Constants;
using OrderingService.AppCore.Helpers;

namespace OrderingService.AppCore.UseCases.Commands
{
    public class NotifyOrderCreated
    {
        public record Command : ICreateCommand<NotifyOrderCreatedDto, bool>
        {
            public NotifyOrderCreatedDto Model { get; init; } = default!;

            internal class Validator : AbstractValidator<Command>
            {
                public Validator()
                {
                    RuleFor(v => v.Model.BuyerEmail)
                        .NotEmpty();

                    RuleFor(v => v.Model.OwnerEmail)
                        .NotEmpty();

                    RuleFor(v => v.Model.ProductName)
                        .NotEmpty();

                    RuleFor(v => v.Model.OrderId)
                        .NotEmpty();
                }
            }

            internal class Handler : IRequestHandler<Command, ResultModel<bool>>
            {
                private readonly IOrderRepository _repository;

                public Handler(IOrderRepository orderRepository)
                {
                    _repository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
                }

                public async Task<ResultModel<bool>> Handle(Command request, CancellationToken cancellationToken)
                {
                    var order = request.Model;
                    var buyerNotification = CreateBuyerNotificationEvent(order.BuyerEmail, order.ProductName, order.OrderId);
                    var ownerNotification = CreateOwnerNotificationEvent(order.OwnerEmail, order.ProductName);

                    await Task.WhenAll(new[]
                    {
                        _repository.PublishNotificationEvent(buyerNotification),
                        _repository.PublishNotificationEvent(ownerNotification)
                    });

                    return ResultModel<bool>.Create(true);
                }

                private NotificationIntegrationEvent CreateBuyerNotificationEvent(
                      string email
                    , string product
                    , Guid orderId
                    )
                {
                    var body = _repository.GetEmailBodyForBuyer();
                    body = body.Replace(EmailPlaceHolderConstants.Email, EmailHelper.EncryptEmail(email))
                               .Replace(EmailPlaceHolderConstants.Product, product)
                               .Replace(EmailPlaceHolderConstants.Order, $"{orderId}");

                    return new NotificationIntegrationEvent
                    {
                        From = _repository.GetDefaultSenderEmail(),
                        To = email,
                        Subject = _repository.GetEmailSubjectForBuyer(),
                        Body = body,
                    };
                }

                private NotificationIntegrationEvent CreateOwnerNotificationEvent(
                      string email
                    , string product
                    )
                {
                    var body = _repository.GetEmailBodyForOwner();
                    body = body.Replace(EmailPlaceHolderConstants.Email, EmailHelper.EncryptEmail(email))
                               .Replace(EmailPlaceHolderConstants.Product, product);

                    return new NotificationIntegrationEvent
                    {
                        From = _repository.GetDefaultSenderEmail(),
                        To = email,
                        Subject = _repository.GetEmailSubjectForOwner(),
                        Body = body,
                    };
                }
            }
        }
    }
}
