using Microsoft.EntityFrameworkCore.Migrations;

namespace GoldPriceOracle.Connection.Database.Migrations
{
    public partial class AddingNewColumnInNodeData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MnemonicPhraseEncrypted",
                table: "NodeData",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MnemonicPhraseEncrypted",
                table: "NodeData");
        }
    }
}
