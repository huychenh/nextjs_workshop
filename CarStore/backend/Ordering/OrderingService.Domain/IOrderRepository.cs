using OrderingService.AppCore.Core;
using NotificationService.Shared.Events;
using OrderingService.Shared.DTO;

namespace OrderingService.AppCore
{
    public interface IOrderRepository
    {
        Task<Guid> Add(Order brand);

        Task PublishNotificationEvent(NotificationIntegrationEvent @event);

        string GetDefaultSenderEmail();

        string GetDefaultSender();

        string GetEmailSubjectForOwner();

        string GetEmailSubjectForBuyer();

        string GetEmailBodyForOwner();

        string GetEmailBodyForBuyer();

        Task<IEnumerable<OrderDto>> GetOrdersByCustomerId(Guid id);
    }
}