using CarStore.AppContracts.Dtos;
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

        public static Order Create(CreateOrderDto dto, string buyerEmail, string ownerEmail)
        {
            var order = new Order
            {
                ProductId = dto.ProductId,
                ProductName = dto.ProductName,
                Price = dto.Price,
                BuyerId = dto.BuyerId,
                OwnerId = dto.OwnerId,
                PictureUrl = dto.PictureUrl,
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
