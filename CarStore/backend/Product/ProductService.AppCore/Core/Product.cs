using CarStore.IntegrationEvents.Product;
using N8T.Core.Domain;

namespace ProductService.AppCore.Core
{
    public class Product : EntityRootBase
    {
        public string Name { get; private init; } = default!;
        public bool Active { get; private init; }
        public int Quantity { get; private init; }
        public decimal Cost { get; private init; }

        public static Product Create(string name, int quantity, decimal cost)
        {
            return Create(Guid.NewGuid(), name, quantity, cost);
        }

        public static Product Create(Guid id, string name, int quantity, decimal cost)
        {
            Product product = new()
            {
                Id = id,
                Name = name,
                Quantity = quantity,
                Created = DateTime.UtcNow,
                Active = true,
                Cost = cost,
            };

            product.AddDomainEvent(new ProductCreatedIntegrationEvent
            {
                Id = product.Id,
                Name = product.Name,
                Quantity = product.Quantity,
                ProductCost = product.Cost,
            });

            return product;
        }
    }
}
