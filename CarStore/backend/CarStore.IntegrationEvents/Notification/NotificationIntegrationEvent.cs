using N8T.Core.Domain;

namespace CarStore.IntegrationEvents.Notification
{
    public class NotificationIntegrationEvent : EventBase
    {
        public string From { get; set; }

        public string To { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public override string[] Topics => new[] { "notification" };
    }
}
