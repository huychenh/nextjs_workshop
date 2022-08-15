namespace NotificationService.AppCore.Dtos
{
    public class EmailConfigOptions
    {
        public string ApiKey { get; set; }

        public string From { get; set; }

        public string FromDisplayName { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }
    }
}
