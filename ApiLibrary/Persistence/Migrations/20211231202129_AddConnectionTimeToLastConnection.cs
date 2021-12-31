using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiLibrary.Persistence.Migrations
{
    public partial class AddConnectionTimeToLastConnection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ConnectionTime",
                table: "LastConnections",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConnectionTime",
                table: "LastConnections");
        }
    }
}
