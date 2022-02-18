using Microsoft.EntityFrameworkCore.Migrations;

namespace GoldPriceOracle.Connection.Database.Migrations
{
    public partial class ChangingIdColunsName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Key",
                table: "FiatCurrencies",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "Key",
                table: "Assets",
                newName: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "FiatCurrencies",
                newName: "Key");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Assets",
                newName: "Key");
        }
    }
}
