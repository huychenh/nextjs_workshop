using N8T.Core.Domain;
using OrderingService.Shared.Events;

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

        public Guid ProductId { get; private set; }

        public decimal Price { get; private set; }

        public string? ProductName { get; private set; }

        public Guid BuyerId { get; private set; }

        public Guid OwnerId { get; private set; }

        public string? PictureUrl { get; private set; }

        public OrderStatus Status { get; private set; }

        public static Order Create(
            Guid productId,
            string? productName,
            decimal price,
            Guid ownerId,
            string? ownerEmail,
            string? pictureUrl,
            Guid buyerId,
            string? buyerEmail)
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
                ProductName = order.ProductName,
                BuyerEmail = buyerEmail,
                OwnerEmail = ownerEmail,
            });

            return order;
        }
    }
}
