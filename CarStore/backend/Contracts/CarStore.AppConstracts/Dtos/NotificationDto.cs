using System.Text.Json.Serialization;

namespace CarStore.AppContracts.Dtos
{
    public class NotificationDto
    {
        public string To { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        [JsonIgnore]
        public string fromEmail { get; set; } = string.Empty;
        [JsonIgnore]
        public string fromName { get; set; } = string.Empty;
    }
}
