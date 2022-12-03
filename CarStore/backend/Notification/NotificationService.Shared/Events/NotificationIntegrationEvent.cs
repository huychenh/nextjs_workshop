using N8T.Core.Domain;

namespace NotificationService.Shared.Events
{
    public class NotificationIntegrationEvent : EventBase
    {
        public string From { get; set; }

        public string FromName { get; set; }

        public string To { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public override string[] Topics => new[] { "notification" };
    }
}
