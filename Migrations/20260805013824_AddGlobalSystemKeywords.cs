using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Archivyn.Migrations
{
    /// <inheritdoc />
    public partial class AddGlobalSystemKeywords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "KEYTYPETABLE",
                keyColumn: "keytypenum",
                keyValue: 1L,
                column: "keytypename",
                value: ">> Document Date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "KEYTYPETABLE",
                keyColumn: "keytypenum",
                keyValue: 1L,
                column: "keytypename",
                value: "Document Date");
        }
    }
}
