using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Archivyn.Migrations
{
    /// <inheritdoc />
    public partial class InitialArchivynSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KEYTYPETABLE",
                columns: table => new
                {
                    keytypenum = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    keytypename = table.Column<string>(type: "character varying(51)", maxLength: 51, nullable: false),
                    keytypemask = table.Column<string>(type: "character varying(51)", maxLength: 51, nullable: true),
                    keytypeflags = table.Column<long>(type: "bigint", nullable: false),
                    datatype = table.Column<long>(type: "bigint", nullable: false),
                    keytypelen = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KEYTYPETABLE", x => x.keytypenum);
                });

            migrationBuilder.CreateTable(
                name: "KEYWORDSET",
                columns: table => new
                {
                    keysettablenum = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    keysetname = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    iskeytypegroup = table.Column<long>(type: "bigint", nullable: false),
                    flags = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KEYWORDSET", x => x.keysettablenum);
                });

            migrationBuilder.CreateTable(
                name: "KEYWORDSETKEYTYPE",
                columns: table => new
                {
                    keysettablenum = table.Column<long>(type: "bigint", nullable: false),
                    keytypenum = table.Column<long>(type: "bigint", nullable: false),
                    displayorder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KEYWORDSETKEYTYPE", x => new { x.keysettablenum, x.keytypenum });
                    table.ForeignKey(
                        name: "FK_KEYWORDSETKEYTYPE_KEYTYPETABLE_keytypenum",
                        column: x => x.keytypenum,
                        principalTable: "KEYTYPETABLE",
                        principalColumn: "keytypenum",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_KEYWORDSETKEYTYPE_KEYWORDSET_keysettablenum",
                        column: x => x.keysettablenum,
                        principalTable: "KEYWORDSET",
                        principalColumn: "keysettablenum",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KEYTYPETABLE_keytypename",
                table: "KEYTYPETABLE",
                column: "keytypename",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KEYWORDSET_keysetname",
                table: "KEYWORDSET",
                column: "keysetname",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KEYWORDSETKEYTYPE_keysettablenum_displayorder",
                table: "KEYWORDSETKEYTYPE",
                columns: new[] { "keysettablenum", "displayorder" });

            migrationBuilder.CreateIndex(
                name: "IX_KEYWORDSETKEYTYPE_keytypenum",
                table: "KEYWORDSETKEYTYPE",
                column: "keytypenum");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KEYWORDSETKEYTYPE");

            migrationBuilder.DropTable(
                name: "KEYTYPETABLE");

            migrationBuilder.DropTable(
                name: "KEYWORDSET");
        }
    }
}
