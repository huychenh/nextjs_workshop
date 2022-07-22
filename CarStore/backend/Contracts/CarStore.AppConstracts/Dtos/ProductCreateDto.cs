namespace CarStore.AppContracts.Dtos
{
    public class ProductCreateDto
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
    }
}
