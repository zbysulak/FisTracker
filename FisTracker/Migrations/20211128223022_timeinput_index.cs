using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FisTracker.Migrations
{
    public partial class timeinput_index : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "Out",
                table: "TimeInputs",
                type: "time(6)",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "time(6)");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "LunchOut",
                table: "TimeInputs",
                type: "time(6)",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "time(6)");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "LunchIn",
                table: "TimeInputs",
                type: "time(6)",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "time(6)");

            migrationBuilder.CreateIndex(
                name: "IX_TimeInputs_Date_UserId",
                table: "TimeInputs",
                columns: new[] { "Date", "UserId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TimeInputs_Date_UserId",
                table: "TimeInputs");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "Out",
                table: "TimeInputs",
                type: "time(6)",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0),
                oldClrType: typeof(TimeSpan),
                oldType: "time(6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "LunchOut",
                table: "TimeInputs",
                type: "time(6)",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0),
                oldClrType: typeof(TimeSpan),
                oldType: "time(6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "LunchIn",
                table: "TimeInputs",
                type: "time(6)",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0),
                oldClrType: typeof(TimeSpan),
                oldType: "time(6)",
                oldNullable: true);
        }
    }
}
