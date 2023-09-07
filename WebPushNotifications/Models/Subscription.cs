using System;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace WebPushNotifications.Models
{
    [Index("Endpoint", IsUnique = true)]
    public class Subscription
	{
		public int Id { get; set; }

		public string Endpoint { get; set; } = "";

		public SubscriptionKeys? Keys { get; set; }

		public Subscription()
		{
		}

		public WebPush.PushSubscription ToWebPushSubscription()
		{

			return new WebPush.PushSubscription(Endpoint, Keys?.P256dh, Keys?.Auth);
		}
	}

	public class SubscriptionKeys
	{
		public int Id { get; set; }

		public string Auth { get; set; } = "";

		public string P256dh { get; set; } = "";

        [JsonIgnore]
        public int SubscriptionId { get; set; }

		[JsonIgnore]
		public Subscription Subscription { get; set; } = null!;

		public SubscriptionKeys() { }
	}
}

