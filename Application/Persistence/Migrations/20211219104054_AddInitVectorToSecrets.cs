using Microsoft.EntityFrameworkCore.Migrations;

namespace Application.Persistence.Migrations
{
    public partial class AddInitVectorToSecrets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Iv",
                table: "Secrets",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Iv",
                table: "Secrets");
        }
    }
}
