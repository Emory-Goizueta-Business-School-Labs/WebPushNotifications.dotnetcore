using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebPushNotifications.Models
{
    public class Notification
	{
		[JsonPropertyName("title")]
		public string Title { get; set; } = "";
        [JsonPropertyName("body")]
        public string Body { get; set; } = "";

        public string GetPayload()
		{
			string payload = JsonSerializer.Serialize(this);
			return payload;
        }

		public Notification()
		{
		}
	}
}

