using N8T.Core.Domain;
using ProductService.Shared.Events;

namespace ProductService.AppCore.Core
{
    public class Product : EntityRootBase
    {
        public string Name { get; private set; } = string.Empty;

        public decimal Price { get; private set; }

        public string Model { get; private set; } = string.Empty;

        public Transmission Transmission { get; private set; }

        public string? MadeIn { get; private set; }

        public int SeatingCapacity { get; private set; }

        public int KmDriven { get; private set; }

        public int Year { get; private set; }

        public FuelType FuelType { get; private set; }

        public string? Category { get; private set; }

        public string? Color { get; private set; }

        public string? Description { get; private set; }

        public bool HasInstallment { get; private set; }

        public bool Active { get; private set; }

        public bool Verified { get; private set; }

        public Guid OwnerId { get; private set; }

        public Guid BrandId { get; private set; }

        public Brand Brand { get; private set; }

        public ICollection<string>? Images { get; set; }

        public static Product Create(
            string name,
            decimal price,
            string model,
            Transmission transmission,
            string? madeIn,
            int seatingCapacity,
            int kmDriven,
            int year,
            FuelType fuelType,
            string? category,
            string? color,
            string? description,
            bool hasInstallment,
            IEnumerable<string> images,
            Guid brandId,
            Guid ownerId)
        {
            // Assume that inputs are valid because of the Validator
            Product product = new()
            {
                Id = Guid.NewGuid(),
                Name = name,
                Price = price,
                BrandId = brandId,
                Model = model,
                Transmission = transmission,
                MadeIn = madeIn,
                SeatingCapacity = seatingCapacity,
                KmDriven = kmDriven,
                Year = year,
                FuelType = fuelType,
                Category = category,
                Color = color,
                Description = description,
                HasInstallment = hasInstallment,
                OwnerId = ownerId,
                Created = DateTime.UtcNow,
                Active = false,
                Verified = false,
                Images = images.ToList(),
            };

            product.AddDomainEvent(new ProductCreatedIntegrationEvent
            {
                Id = product.Id,
                OwnerId = product.OwnerId,
            });

            return product;
        }
    }
}
