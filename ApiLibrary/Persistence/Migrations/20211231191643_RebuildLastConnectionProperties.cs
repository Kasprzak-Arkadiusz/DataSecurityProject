using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiLibrary.Persistence.Migrations
{
    public partial class RebuildLastConnectionProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeviceDetails",
                table: "LastConnections");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "LastConnections");

            migrationBuilder.AddColumn<string>(
                name: "BrowserName",
                table: "LastConnections",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "LastConnections",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "LastConnections",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeviceType",
                table: "LastConnections",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PlatformName",
                table: "LastConnections",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "LastConnections",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BrowserName",
                table: "LastConnections");

            migrationBuilder.DropColumn(
                name: "City",
                table: "LastConnections");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "LastConnections");

            migrationBuilder.DropColumn(
                name: "DeviceType",
                table: "LastConnections");

            migrationBuilder.DropColumn(
                name: "PlatformName",
                table: "LastConnections");

            migrationBuilder.DropColumn(
                name: "Region",
                table: "LastConnections");

            migrationBuilder.AddColumn<string>(
                name: "DeviceDetails",
                table: "LastConnections",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "LastConnections",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true);
        }
    }
}
