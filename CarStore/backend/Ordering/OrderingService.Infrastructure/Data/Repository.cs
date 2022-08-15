using CarStore.IntegrationEvents.Notification;
using Microsoft.Extensions.Options;
using N8T.Infrastructure.Bus;
using OrderingService.AppCore;
using OrderingService.AppCore.Core;
using OrderingService.AppCore.Dtos;

namespace OrderingService.Infrastructure.Data
{
    public class Repository : IOrderRepository
    {
        private readonly MainDbContext _dbContext;

        private readonly IEventBus _eventBus;

        private readonly NotificationConfigOptions _notificationConfig;

        public Repository(MainDbContext dbContext, IEventBus eventBus
            , IOptions<NotificationConfigOptions> notificationConfig)
        {
            _dbContext = dbContext;
            _eventBus = eventBus;
            _notificationConfig = notificationConfig.Value;
        }

        public async Task<Guid> Add(Order order)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }

            _dbContext.Orders.Add(order);

            await _dbContext.SaveChangesAsync();

            return order.Id;
        }

        public string GetDefaultSenderEmail() => _notificationConfig.DefaultSenderEmail;

        public string GetEmailBodyForBuyer() => _notificationConfig.EmailBodyForBuyer;

        public string GetEmailBodyForOwner() => _notificationConfig.EmailBodyForOwner;

        public string GetEmailSubjectForBuyer() => _notificationConfig.EmailSubjectForBuyer;

        public string GetEmailSubjectForOwner() => _notificationConfig.EmailSubjectForOwner;

        public Task PublishNotificationEvent(NotificationIntegrationEvent @event)
        {
            return _eventBus.PublishAsync(@event, @event.Topics);
        }
    }
}
