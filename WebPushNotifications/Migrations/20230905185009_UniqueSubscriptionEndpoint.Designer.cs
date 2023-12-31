﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebPushNotifications.Data;

#nullable disable

namespace WebPushNotifications.Migrations
{
    [DbContext(typeof(WebPushNotificationsContext))]
    [Migration("20230905185009_UniqueSubscriptionEndpoint")]
    partial class UniqueSubscriptionEndpoint
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.10");

            modelBuilder.Entity("WebPushNotifications.Models.Subscription", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Endpoint")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Endpoint")
                        .IsUnique();

                    b.ToTable("Subscriptions");
                });

            modelBuilder.Entity("WebPushNotifications.Models.SubscriptionKeys", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Auth")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("P256dh")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("SubscriptionId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("SubscriptionId")
                        .IsUnique();

                    b.ToTable("SubscriptionKeys");
                });

            modelBuilder.Entity("WebPushNotifications.Models.SubscriptionKeys", b =>
                {
                    b.HasOne("WebPushNotifications.Models.Subscription", "Subscription")
                        .WithOne("Keys")
                        .HasForeignKey("WebPushNotifications.Models.SubscriptionKeys", "SubscriptionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Subscription");
                });

            modelBuilder.Entity("WebPushNotifications.Models.Subscription", b =>
                {
                    b.Navigation("Keys");
                });
#pragma warning restore 612, 618
        }
    }
}
