using CarStore.IntegrationEvents.Notification;
using OrderingService.AppCore.Core;

namespace OrderingService.AppCore
{
    public interface IOrderRepository
    {
        Task<Guid> Add(Order brand);

        Task PublishNotificationEvent(NotificationIntegrationEvent @event);
        
        string GetDefaultSenderEmail();
        
        string GetEmailSubjectForOwner();
        
        string GetEmailSubjectForBuyer();
        
        string GetEmailBodyForOwner();
        
        string GetEmailBodyForBuyer();
    }
}