using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Archivyn.Migrations
{
    /// <inheritdoc />
    public partial class AddSystemDocumentConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "keytypeflags",
                table: "KEYTYPETABLE");

            migrationBuilder.DropColumn(
                name: "keytypemask",
                table: "KEYTYPETABLE");

            migrationBuilder.AddColumn<bool>(
                name: "IsSystem",
                table: "KEYWORDSETKEYTYPE",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSystem",
                table: "KEYWORDSET",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "datatype",
                table: "KEYTYPETABLE",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<bool>(
                name: "AddToAllDocumentTypes",
                table: "KEYTYPETABLE",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "AllDocumentTypesDisplayOrder",
                table: "KEYTYPETABLE",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsRequiredOnAllDocumentTypes",
                table: "KEYTYPETABLE",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSystem",
                table: "KEYTYPETABLE",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSystem",
                table: "ITEMTYPEGROUP",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSystem",
                table: "DOCTYPEKEYTYPEGROUP",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRequired",
                table: "DOCTYPEKEYTYPE",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSystem",
                table: "DOCTYPEKEYTYPE",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSystem",
                table: "DOCTYPE",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "ITEMTYPEGROUP",
                columns: new[] { "itemtypegroupnum", "IsSystem", "itemtypegroupname" },
                values: new object[] { 1L, true, "System Documents" });

            migrationBuilder.InsertData(
                table: "KEYTYPETABLE",
                columns: new[] { "keytypenum", "AddToAllDocumentTypes", "AllDocumentTypesDisplayOrder", "datatype", "IsRequiredOnAllDocumentTypes", "IsSystem", "keytypelen", "keytypename" },
                values: new object[] { 1L, true, 1, 3, true, true, 10L, "Document Date" });

            migrationBuilder.InsertData(
                table: "KEYTYPETABLE",
                columns: new[] { "keytypenum", "AddToAllDocumentTypes", "AllDocumentTypesDisplayOrder", "datatype", "IsRequiredOnAllDocumentTypes", "keytypelen", "keytypename" },
                values: new object[] { 2L, true, 2, 1, false, 250L, "Description" });

            migrationBuilder.InsertData(
                table: "DOCTYPE",
                columns: new[] { "itemtypenum", "autonamestring", "IsSystem", "itemtypegroupnum", "itemtypename" },
                values: new object[] { 1L, null, true, 1L, "Unindexed" });

            migrationBuilder.InsertData(
                table: "DOCTYPEKEYTYPE",
                columns: new[] { "itemtypenum", "keytypenum", "displayorder", "IsRequired", "IsSystem" },
                values: new object[] { 1L, 2L, 2, true, true });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "DOCTYPEKEYTYPE",
                keyColumns: new[] { "itemtypenum", "keytypenum" },
                keyValues: new object[] { 1L, 2L });

            migrationBuilder.DeleteData(
                table: "KEYTYPETABLE",
                keyColumn: "keytypenum",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "DOCTYPE",
                keyColumn: "itemtypenum",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "KEYTYPETABLE",
                keyColumn: "keytypenum",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "ITEMTYPEGROUP",
                keyColumn: "itemtypegroupnum",
                keyValue: 1L);

            migrationBuilder.DropColumn(
                name: "IsSystem",
                table: "KEYWORDSETKEYTYPE");

            migrationBuilder.DropColumn(
                name: "IsSystem",
                table: "KEYWORDSET");

            migrationBuilder.DropColumn(
                name: "AddToAllDocumentTypes",
                table: "KEYTYPETABLE");

            migrationBuilder.DropColumn(
                name: "AllDocumentTypesDisplayOrder",
                table: "KEYTYPETABLE");

            migrationBuilder.DropColumn(
                name: "IsRequiredOnAllDocumentTypes",
                table: "KEYTYPETABLE");

            migrationBuilder.DropColumn(
                name: "IsSystem",
                table: "KEYTYPETABLE");

            migrationBuilder.DropColumn(
                name: "IsSystem",
                table: "ITEMTYPEGROUP");

            migrationBuilder.DropColumn(
                name: "IsSystem",
                table: "DOCTYPEKEYTYPEGROUP");

            migrationBuilder.DropColumn(
                name: "IsRequired",
                table: "DOCTYPEKEYTYPE");

            migrationBuilder.DropColumn(
                name: "IsSystem",
                table: "DOCTYPEKEYTYPE");

            migrationBuilder.DropColumn(
                name: "IsSystem",
                table: "DOCTYPE");

            migrationBuilder.AlterColumn<long>(
                name: "datatype",
                table: "KEYTYPETABLE",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<long>(
                name: "keytypeflags",
                table: "KEYTYPETABLE",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "keytypemask",
                table: "KEYTYPETABLE",
                type: "character varying(51)",
                maxLength: 51,
                nullable: true);
        }
    }
}
