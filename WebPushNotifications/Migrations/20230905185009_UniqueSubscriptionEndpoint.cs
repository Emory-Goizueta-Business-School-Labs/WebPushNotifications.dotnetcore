using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebPushNotifications.Migrations
{
    /// <inheritdoc />
    public partial class UniqueSubscriptionEndpoint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_Endpoint",
                table: "Subscriptions",
                column: "Endpoint",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_Endpoint",
                table: "Subscriptions");
        }
    }
}
