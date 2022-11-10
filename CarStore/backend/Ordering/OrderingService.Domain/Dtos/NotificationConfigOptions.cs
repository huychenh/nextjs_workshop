namespace OrderingService.AppCore.Dtos
{
    public class NotificationConfigOptions
    {
        public string DefaultSenderEmail { get; set; }

        public string DefaultSender { get; set; }

        public string EmailBodyForBuyer { get; set; }

        public string EmailBodyForOwner { get; set; }

        public string EmailSubjectForBuyer { get; set; }

        public string EmailSubjectForOwner { get; set; }
    }
}
