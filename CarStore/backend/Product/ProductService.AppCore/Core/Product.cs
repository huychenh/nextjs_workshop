using CarStore.AppContracts.Dtos;
using CarStore.IntegrationEvents.Product;
using N8T.Core.Domain;

namespace ProductService.AppCore.Core
{
    public class Product : EntityRootBase
    {
        public string Name { get; private set; }

        public decimal Price { get; private set; }

        public string Model { get; private set; }

        public Transmission Transmission { get; private set; }

        public string MadeIn { get; private set; }

        public int SeatingCapacity { get; private set; }

        public int KmDriven { get; private set; }

        public int Year { get; private set; }

        public FuelType FuelType { get; private set; }

        public string Category { get; private set; }

        public string Color { get; private set; }

        public string Description { get; private set; }

        public bool HasInstallment { get; private set; }

        public bool Active { get; private set; }

        public bool Verified { get; private set; }

        public Guid OwnerId { get; private set; }

        public Guid BrandId { get; private set; }

        public Brand Brand { get; private set; }

        public ICollection<string>? Images { get; set; }

        public static Product Create(ProductCreateDto dto, Guid brandId, Guid ownerId)
        {
            Product product = new()
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Price = dto.Price,
                BrandId = brandId,
                Model = dto.Model,
                Transmission = (Transmission)Enum.Parse(typeof(Transmission), dto.Transmission),
                MadeIn = dto.MadeIn,
                SeatingCapacity = dto.SeatingCapacity,
                KmDriven = dto.KmDriven,
                Year = dto.Year,
                FuelType = (FuelType)Enum.Parse(typeof(FuelType), dto.FuelType),
                Category = dto.Category,
                Color = dto.Color,
                Description = dto.Description,
                HasInstallment = dto.HasInstallment,
                OwnerId = ownerId,
                Created = DateTime.UtcNow,
                Active = false,
                Verified = false,
                Images = dto.Images,
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
