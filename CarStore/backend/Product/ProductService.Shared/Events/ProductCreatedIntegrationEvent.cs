using N8T.Core.Domain;

namespace ProductService.Shared.Events
{
    public class ProductCreatedIntegrationEvent : EventBase
    {
        public Guid Id { get; set; }

        public Guid OwnerId { get; set; }

        public override string[] Topics => new[] { "product" };
    }
}
