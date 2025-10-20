using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstateManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddUserPendingUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasPendingUpdates",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PendingUpdates",
                table: "Users",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasPendingUpdates",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PendingUpdates",
                table: "Users");
        }
    }
}
