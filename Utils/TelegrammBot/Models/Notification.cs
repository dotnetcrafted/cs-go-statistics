using Newtonsoft.Json;

namespace TelegramBot.Models
{
    public class Notification
    {
        public string Text { get; set; }
        
        [JsonProperty("Type")]
        public NotificationType Type { get; set; }
    }

    public class NotificationType
    {
        public string Name { get; set; }

        public int TypeId { get; set; }
    }
}