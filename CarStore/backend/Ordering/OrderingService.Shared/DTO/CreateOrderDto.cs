namespace OrderingService.Shared.DTO
{
    public class CreateOrderDto
    {
        public Guid ProductId { get; set; }

        public decimal Price { get; set; }

        public string ProductName { get; set; }

        public Guid OwnerId { get; set; }

        public string BuyerEmail { get; set; }

        public string? PictureUrl { get; set; }
    }
}
