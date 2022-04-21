namespace CarStore.AppContracts.Dtos
{
    public class ProductDto
    {
        public string Name { get; set; } = default!;

        public bool Active { get; set; }
        
        public int Quantity { get; set; }
        
        public decimal Cost { get; set; }
        
        public Guid ProductCodeId { get; set; }
        
        public Guid Id { get; set; }
        
        public DateTime Created { get; set; }
        
        public DateTime? Updated { get; set; }
    }
}
