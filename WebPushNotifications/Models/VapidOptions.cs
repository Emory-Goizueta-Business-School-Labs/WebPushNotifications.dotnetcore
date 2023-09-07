using System;
namespace WebPushNotifications.Models
{
	public class VapidOptions
	{
        public const string Vapid = "Vapid";

        public string Subject { get; set; } = "";
		public string Public { get; set; } = "";
        public string Private { get; set; } = "";

        public VapidOptions()
		{
		}
	}
}

