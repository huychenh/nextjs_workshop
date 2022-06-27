namespace CarStore.AppContracts.Dtos
{
    public class ProductDto
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Brand { get; set; }

        public string Model { get; set; }

        public string Transmission { get; set; }

        public string MadeIn { get; set; }

        public int SeatingCapacity { get; set; }

        public int KmDriven { get; set; }

        public int Year { get; set; }

        public string FuelType { get; set; }

        public string Category { get; set; }

        public string Color { get; set; }

        public string Description { get; set; }

        public bool HasInstallment { get; set; }

        public bool Verified { get; set; }

        public string OwnerName { get; set; }

        public Guid Id { get; set; }

        public bool Active { get; set; }

        public DateTime Created { get; set; }
        
        public DateTime? Updated { get; set; }

        public int TotalPages { get; set; }
    }
}
