using CarStore.IntegrationEvents.Notification;
using Microsoft.Extensions.Options;
using N8T.Infrastructure.Bus;
using CarStore.AppContracts.Dtos;
using Microsoft.EntityFrameworkCore;
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

        public string GetDefaultSender() => _notificationConfig.DefaultSender;

        public string GetEmailBodyForBuyer() => _notificationConfig.EmailBodyForBuyer;

        public string GetEmailBodyForOwner() => _notificationConfig.EmailBodyForOwner;

        public string GetEmailSubjectForBuyer() => _notificationConfig.EmailSubjectForBuyer;

        public string GetEmailSubjectForOwner() => _notificationConfig.EmailSubjectForOwner;

        public Task PublishNotificationEvent(NotificationIntegrationEvent @event)
        {
            return _eventBus.PublishAsync(@event, @event.Topics);
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersByCustomerId(Guid id)
        {
            var orders = await _dbContext.Orders
                .Where(x => x.BuyerId == id)
                .Select(x => new OrderDto
                {
                    Id = x.Id,
                    ProductId = x.ProductId,
                    OwnerId = x.OwnerId,
                    BuyerId = x.BuyerId,
                    Price = x.Price,
                    ProductName = x.ProductName,
                    PictureUrl = x.PictureUrl,
                    Date = x.Created,
                })
                .ToListAsync();

            return orders;
        }
    }
}
