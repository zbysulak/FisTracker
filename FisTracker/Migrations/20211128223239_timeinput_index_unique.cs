using Microsoft.EntityFrameworkCore.Migrations;

namespace FisTracker.Migrations
{
    public partial class timeinput_index_unique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TimeInputs_Date_UserId",
                table: "TimeInputs");

            migrationBuilder.CreateIndex(
                name: "IX_TimeInputs_Date_UserId",
                table: "TimeInputs",
                columns: new[] { "Date", "UserId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TimeInputs_Date_UserId",
                table: "TimeInputs");

            migrationBuilder.CreateIndex(
                name: "IX_TimeInputs_Date_UserId",
                table: "TimeInputs",
                columns: new[] { "Date", "UserId" });
        }
    }
}
