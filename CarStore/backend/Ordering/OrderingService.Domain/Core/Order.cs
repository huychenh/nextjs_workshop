using CarStore.IntegrationEvents.Ordering;
using N8T.Core.Domain;

namespace OrderingService.AppCore.Core
{
    public enum OrderStatus
    {
        Submitted,
        Cancelled,
    }

    public class Order : EntityRootBase
    {
        private DateTime _orderDate;

        public int ProductId { get; private set; }

        public decimal Price { get; private set; }

        public string? ProductName { get; private set; }

        public int BuyerId { get; private set; }

        public int OwnerId { get; private set; }

        public string? PictureUrl { get; private set; }

        public OrderStatus Status { get; private set; }

        public static Order Create(int productId, string productName, string pictureUrl, int buyerId, int ownerId,
            decimal price, string buyerEmail, string ownerEmail)
        {
            var order = new Order
            {
                ProductId = productId,
                ProductName = productName,
                Price = price,
                BuyerId = buyerId,
                OwnerId = ownerId,
                PictureUrl = pictureUrl,
                Id = Guid.NewGuid(),
                _orderDate = DateTime.UtcNow,
                Status = OrderStatus.Submitted,
            };

            order.AddDomainEvent(new OrderCreatedIntegrationEvent
            {
                OrderId = order.Id,
                ProductName = productName,
                BuyerEmail = buyerEmail,
                OwnerEmail = ownerEmail,
            });

            return order;
        }
    }
}
