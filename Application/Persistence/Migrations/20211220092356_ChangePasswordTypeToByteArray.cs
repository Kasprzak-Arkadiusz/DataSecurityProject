using Microsoft.EntityFrameworkCore.Migrations;

namespace Application.Persistence.Migrations
{
    public partial class ChangePasswordTypeToByteArray : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn("Password", "Users");
            migrationBuilder.AddColumn<byte[]>("Password", "Users", "varbinary(128)", maxLength: 128, nullable: false);

            migrationBuilder.DropColumn("MasterPassword", "Users");
            migrationBuilder.AddColumn<byte[]>("MasterPassword", "Users", "varbinary(128)", maxLength: 128, nullable: false);

            migrationBuilder.DropColumn("Password", "Secrets");
            migrationBuilder.AddColumn<byte[]>("Password", "Secrets", "varbinary(128)", maxLength: 128, nullable: false);

            migrationBuilder.DropColumn("Iv", "Secrets");
            migrationBuilder.AddColumn<byte[]>("Iv", "Secrets", "varbinary(128)", maxLength: 128, nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn("Password", "Users");
            migrationBuilder.AddColumn<string>("Password", "Users", "nvarchar(128)", maxLength: 128, nullable: false);

            migrationBuilder.DropColumn("MasterPassword", "Users");
            migrationBuilder.AddColumn<string>("MasterPassword", "Users", "nvarchar(128)", maxLength: 128, nullable: false);

            migrationBuilder.DropColumn("Password", "Secrets");
            migrationBuilder.AddColumn<string>("Password", "Secrets", "nvarchar(128)", maxLength: 128, nullable: false);

            migrationBuilder.DropColumn("Iv", "Secrets");
            migrationBuilder.AddColumn<string>("Iv", "Secrets", "nvarchar(128)", maxLength: 128, nullable: false);
        }
    }
}