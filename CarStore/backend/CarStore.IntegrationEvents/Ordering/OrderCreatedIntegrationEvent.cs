using N8T.Core.Domain;

namespace CarStore.IntegrationEvents.Ordering
{
    public class OrderCreatedIntegrationEvent : EventBase
    {
        public Guid OrderId { get; set; }

        public string? BuyerEmail { get; set; }

        public string? OwnerEmail { get; set; }

        public string? ProductName { get; set; }

        public override string[] Topics => new[] { "order" };
    }
}
