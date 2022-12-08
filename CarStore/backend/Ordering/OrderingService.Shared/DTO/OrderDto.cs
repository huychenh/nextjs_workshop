namespace OrderingService.Shared.DTO
{
    public class OrderDto
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public decimal Price { get; set; }

        public string ProductName { get; set; }

        public Guid OwnerId { get; set; }

        public Guid BuyerId { get; set; }

        public string? PictureUrl { get; set; }

        public DateTime Date { get; set; }
    }
}
