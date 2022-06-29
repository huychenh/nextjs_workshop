namespace CarStore.AppContracts.Dtos
{
    public class CreateOrderDto
    {
        public int ProductId { get; set; }

        public decimal Price { get; set; }

        public string? ProductName { get; set; }

        public int BuyerId { get; set; }

        public int OwnerId { get; set; }

        public string? PictureUrl { get; set; }
    }
}
