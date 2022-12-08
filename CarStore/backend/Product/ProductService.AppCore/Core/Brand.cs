using N8T.Core.Domain;
using ProductService.Shared.Events;

namespace ProductService.AppCore.Core
{
    public class Brand : EntityRootBase
    {
        public string Name { get; set; } = string.Empty;

        public ICollection<Product> Products { get; set; } = new List<Product>();

        public static Brand Create(string name)
        {
            var brand = new Brand
            {
                Name = name,
            };

            brand.AddDomainEvent(new BrandCreatedIntegrationEvent
            {
                Id = brand.Id
            });

            return brand;
        }
    }
}
