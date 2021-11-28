using Microsoft.EntityFrameworkCore.Migrations;

namespace FisTracker.Migrations
{
    public partial class homeoffice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HomeOffice",
                table: "TimeInputs",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HomeOffice",
                table: "TimeInputs");
        }
    }
}
