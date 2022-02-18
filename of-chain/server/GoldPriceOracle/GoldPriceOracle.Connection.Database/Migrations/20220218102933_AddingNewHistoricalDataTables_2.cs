using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GoldPriceOracle.Connection.Database.Migrations
{
    public partial class AddingNewHistoricalDataTables_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssetHistoricDatas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AssetId = table.Column<short>(type: "smallint", nullable: false),
                    FiatCurrencyId = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetHistoricDatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssetHistoricDatas_Assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Assets",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssetHistoricDatas_FiatCurrencies_FiatCurrencyId",
                        column: x => x.FiatCurrencyId,
                        principalTable: "FiatCurrencies",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssetHistoricDatas_AssetId",
                table: "AssetHistoricDatas",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetHistoricDatas_FiatCurrencyId",
                table: "AssetHistoricDatas",
                column: "FiatCurrencyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssetHistoricDatas");
        }
    }
}
