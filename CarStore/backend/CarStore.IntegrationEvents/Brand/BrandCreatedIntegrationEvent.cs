using N8T.Core.Domain;

namespace CarStore.IntegrationEvents.Brand
{
    public class BrandCreatedIntegrationEvent : EventBase
    {
        public Guid Id { get; set; }

        public override void Flatten()
        {
            MetaData.Add("BrandId", Id);
        }
    }
}
