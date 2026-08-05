using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Archivyn.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAddAll : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "KEYTYPETABLE",
                keyColumn: "keytypenum",
                keyValue: 2L,
                columns: new[] { "AddToAllDocumentTypes", "IsSystem" },
                values: new object[] { true, false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "KEYTYPETABLE",
                keyColumn: "keytypenum",
                keyValue: 2L,
                columns: new[] { "AddToAllDocumentTypes", "IsSystem" },
                values: new object[] { false, true });
        }
    }
}
