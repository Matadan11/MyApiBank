using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyApiBank.Migrations
{
    /// <inheritdoc />
    public partial class Initial1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Deposits",
                table: "Accounts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Withdrawals",
                table: "Accounts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "accrued_interest",
                table: "Accounts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deposits",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "Withdrawals",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "accrued_interest",
                table: "Accounts");
        }
    }
}
