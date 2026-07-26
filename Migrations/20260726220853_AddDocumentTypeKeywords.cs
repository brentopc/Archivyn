using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Archivyn.Migrations
{
    /// <inheritdoc />
    public partial class AddDocumentTypeKeywords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DOCTYPEKEYTYPE",
                columns: table => new
                {
                    itemtypenum = table.Column<long>(type: "bigint", nullable: false),
                    keytypenum = table.Column<long>(type: "bigint", nullable: false),
                    displayorder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DOCTYPEKEYTYPE", x => new { x.itemtypenum, x.keytypenum });
                    table.ForeignKey(
                        name: "FK_DOCTYPEKEYTYPE_DOCTYPE_itemtypenum",
                        column: x => x.itemtypenum,
                        principalTable: "DOCTYPE",
                        principalColumn: "itemtypenum",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DOCTYPEKEYTYPE_KEYTYPETABLE_keytypenum",
                        column: x => x.keytypenum,
                        principalTable: "KEYTYPETABLE",
                        principalColumn: "keytypenum",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DOCTYPEKEYTYPEGROUP",
                columns: table => new
                {
                    itemtypenum = table.Column<long>(type: "bigint", nullable: false),
                    keysettablenum = table.Column<long>(type: "bigint", nullable: false),
                    displayorder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DOCTYPEKEYTYPEGROUP", x => new { x.itemtypenum, x.keysettablenum });
                    table.ForeignKey(
                        name: "FK_DOCTYPEKEYTYPEGROUP_DOCTYPE_itemtypenum",
                        column: x => x.itemtypenum,
                        principalTable: "DOCTYPE",
                        principalColumn: "itemtypenum",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DOCTYPEKEYTYPEGROUP_KEYWORDSET_keysettablenum",
                        column: x => x.keysettablenum,
                        principalTable: "KEYWORDSET",
                        principalColumn: "keysettablenum",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DOCTYPEKEYTYPE_itemtypenum_displayorder",
                table: "DOCTYPEKEYTYPE",
                columns: new[] { "itemtypenum", "displayorder" });

            migrationBuilder.CreateIndex(
                name: "IX_DOCTYPEKEYTYPE_keytypenum",
                table: "DOCTYPEKEYTYPE",
                column: "keytypenum");

            migrationBuilder.CreateIndex(
                name: "IX_DOCTYPEKEYTYPEGROUP_itemtypenum_displayorder",
                table: "DOCTYPEKEYTYPEGROUP",
                columns: new[] { "itemtypenum", "displayorder" });

            migrationBuilder.CreateIndex(
                name: "IX_DOCTYPEKEYTYPEGROUP_keysettablenum",
                table: "DOCTYPEKEYTYPEGROUP",
                column: "keysettablenum");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DOCTYPEKEYTYPE");

            migrationBuilder.DropTable(
                name: "DOCTYPEKEYTYPEGROUP");
        }
    }
}
