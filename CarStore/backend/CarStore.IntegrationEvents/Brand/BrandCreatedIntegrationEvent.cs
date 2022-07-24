using N8T.Core.Domain;

namespace CarStore.IntegrationEvents.Brand
{
    public class BrandCreatedIntegrationEvent : EventBase
    {
        public Guid Id { get; set; }

        public override string[] Topics => new[] { "brand" };
    }
}
