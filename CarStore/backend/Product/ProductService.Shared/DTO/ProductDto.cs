namespace ProductService.Shared.DTO
{
    public class ProductDto
    {
        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public string Brand { get; set; } = string.Empty;

        public string Model { get; set; } = string.Empty;

        public string Transmission { get; set; } = string.Empty;

        public string? MadeIn { get; set; }

        public int SeatingCapacity { get; set; }

        public int KmDriven { get; set; }

        public int Year { get; set; }

        public string FuelType { get; set; } = string.Empty;

        public string? Category { get; set; }

        public string? Color { get; set; }

        public string? Description { get; set; }

        public bool HasInstallment { get; set; }

        public bool Verified { get; set; }

        public string OwnerId { get; set; } = string.Empty;

        public Guid Id { get; set; }

        public bool Active { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Updated { get; set; }

        public string[] Images { get; set; } = Array.Empty<string>();
    }
}
