using N8T.Core.Domain;

namespace OrderingService.Shared.Events
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
