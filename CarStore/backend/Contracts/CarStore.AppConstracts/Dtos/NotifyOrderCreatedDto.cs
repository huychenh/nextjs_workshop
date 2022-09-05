namespace CarStore.AppContracts.Dtos
{
    public class NotifyOrderCreatedDto
    {
        public Guid OrderId { get; set; }

        public string BuyerEmail { get; set; }

        public string OwnerEmail { get; set; }

        public string ProductName { get; set; }
    }
}
