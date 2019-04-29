using Microsoft.EntityFrameworkCore.Migrations;

namespace Iris.Rms.Data.Migrations
{
    public partial class webhookupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RmsId",
                table: "WebHook");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RmsId",
                table: "WebHook",
                nullable: false,
                defaultValue: 0);
        }
    }
}
