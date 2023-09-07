using System;
namespace WebPushNotifications.Models
{
	public class RegisterSubscriptionResponse
	{
		public bool Success { get; set; } = false;
		public RegisterSubscriptionResponse()
		{
		}
	}

	public class RegisterSubscriptionError : RegisterSubscriptionResponse
	{
		public string Error { get; set; }

        public RegisterSubscriptionError(string error) : base()
        {
			this.Success = false;
			this.Error = error;
        }
    }

	public class RegisterSubscriptionSuccess : RegisterSubscriptionResponse
	{
		public Subscription Subscription { get; set; }

        public RegisterSubscriptionSuccess(Subscription subscription) : base()
        {
			this.Success = true;
			this.Subscription = subscription;
        }
    }
}

