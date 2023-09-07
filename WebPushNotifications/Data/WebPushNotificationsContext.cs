using System;
using Microsoft.EntityFrameworkCore;
using WebPushNotifications.Models;

namespace WebPushNotifications.Data
{
	public class WebPushNotificationsContext: DbContext
	{
		public DbSet<Subscription> Subscriptions { get; set; }

		public WebPushNotificationsContext(DbContextOptions<WebPushNotificationsContext> options)
			:base(options)
		{
		}
	}
}

