namespace CarStore.AppContracts.Dtos
{
    public class CreateOrderDto
    {
        public int ProductId { get; set; }

        public decimal Price { get; set; }

        public string ProductName { get; set; }

        public Guid BuyerId { get; set; }

        public Guid OwnerId { get; set; }

        public string PictureUrl { get; set; }
    }
}
