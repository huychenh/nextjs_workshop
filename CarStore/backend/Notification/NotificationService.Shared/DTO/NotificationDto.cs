using System.Text.Json.Serialization;

namespace NotificationService.Shared.DTO
{
    public class NotificationDto
    {
        public string To { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        [JsonIgnore]
        public string From { get; set; } = string.Empty;
        [JsonIgnore]
        public string FromName { get; set; } = string.Empty;
    }
}
