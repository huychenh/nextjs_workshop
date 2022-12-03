using N8T.Core.Domain;

namespace ProductService.Shared.Events
{
    public class BrandCreatedIntegrationEvent : EventBase
    {
        public Guid Id { get; set; }

        public override string[] Topics => new[] { "brand" };
    }
}
