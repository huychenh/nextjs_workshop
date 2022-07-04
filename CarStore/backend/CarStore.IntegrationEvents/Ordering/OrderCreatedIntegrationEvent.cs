using N8T.Core.Domain;

namespace CarStore.IntegrationEvents.Ordering
{
    public class OrderCreatedIntegrationEvent : EventBase
    {
        public Guid OrderId { get; set; }

        public string? BuyerEmail { get; set; }

        public string? OwnerEmail { get; set; }

        public string? ProductName { get; set; }

        public override void Flatten()
        {
            MetaData.Add("OrderId", OrderId);
            MetaData.Add("BuyerEmail", BuyerEmail);
            MetaData.Add("OwnerEmail", OwnerEmail);
            MetaData.Add("ProductName", ProductName);
        }
    }
}
