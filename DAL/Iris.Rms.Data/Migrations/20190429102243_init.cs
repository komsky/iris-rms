using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Iris.Rms.Data.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RmsList",
                columns: table => new
                {
                    RmsId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RmsList", x => x.RmsId);
                });

            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    RmsDeviceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<int>(nullable: false),
                    RmsConfigRmsId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.RmsDeviceId);
                    table.ForeignKey(
                        name: "FK_Devices_RmsList_RmsConfigRmsId",
                        column: x => x.RmsConfigRmsId,
                        principalTable: "RmsList",
                        principalColumn: "RmsId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WebHook",
                columns: table => new
                {
                    WebHookHookId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HookUrl = table.Column<string>(nullable: true),
                    Method = table.Column<string>(nullable: true),
                    Body = table.Column<string>(nullable: true),
                    RmsId = table.Column<int>(nullable: false),
                    RmsDeviceId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebHook", x => x.WebHookHookId);
                    table.ForeignKey(
                        name: "FK_WebHook_Devices_RmsDeviceId",
                        column: x => x.RmsDeviceId,
                        principalTable: "Devices",
                        principalColumn: "RmsDeviceId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Devices_RmsConfigRmsId",
                table: "Devices",
                column: "RmsConfigRmsId");

            migrationBuilder.CreateIndex(
                name: "IX_WebHook_RmsDeviceId",
                table: "WebHook",
                column: "RmsDeviceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WebHook");

            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropTable(
                name: "RmsList");
        }
    }
}
