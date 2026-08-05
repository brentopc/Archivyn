using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Archivyn.Migrations
{
    /// <inheritdoc />
    public partial class AddItemData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "itemdata",
                columns: table => new
                {
                    itemnum = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    itemname = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    itemtypegroupnum = table.Column<long>(type: "bigint", nullable: false),
                    itemtypenum = table.Column<long>(type: "bigint", nullable: false),
                    itemdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    datestored = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    usernum = table.Column<long>(type: "bigint", nullable: true),
                    originalfilename = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    fileextension = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    filesize = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_itemdata", x => x.itemnum);
                    table.ForeignKey(
                        name: "FK_itemdata_DOCTYPE_itemtypenum",
                        column: x => x.itemtypenum,
                        principalTable: "DOCTYPE",
                        principalColumn: "itemtypenum",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_itemdata_ITEMTYPEGROUP_itemtypegroupnum",
                        column: x => x.itemtypegroupnum,
                        principalTable: "ITEMTYPEGROUP",
                        principalColumn: "itemtypegroupnum",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "keyitem",
                columns: table => new
                {
                    keyitemnum = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    itemnum = table.Column<long>(type: "bigint", nullable: false),
                    keytypenum = table.Column<long>(type: "bigint", nullable: false),
                    KeySetTableNum = table.Column<long>(type: "bigint", nullable: true),
                    RecordNum = table.Column<long>(type: "bigint", nullable: true),
                    keyvaluechar = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    keyvaluenum = table.Column<decimal>(type: "numeric", nullable: true),
                    keyvaluedate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_keyitem", x => x.keyitemnum);
                    table.ForeignKey(
                        name: "FK_keyitem_KEYTYPETABLE_keytypenum",
                        column: x => x.keytypenum,
                        principalTable: "KEYTYPETABLE",
                        principalColumn: "keytypenum",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_keyitem_itemdata_itemnum",
                        column: x => x.itemnum,
                        principalTable: "itemdata",
                        principalColumn: "itemnum",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_itemdata_itemtypegroupnum",
                table: "itemdata",
                column: "itemtypegroupnum");

            migrationBuilder.CreateIndex(
                name: "IX_itemdata_itemtypenum",
                table: "itemdata",
                column: "itemtypenum");

            migrationBuilder.CreateIndex(
                name: "IX_keyitem_itemnum_keytypenum",
                table: "keyitem",
                columns: new[] { "itemnum", "keytypenum" });

            migrationBuilder.CreateIndex(
                name: "IX_keyitem_keytypenum_keyvaluechar",
                table: "keyitem",
                columns: new[] { "keytypenum", "keyvaluechar" });

            migrationBuilder.CreateIndex(
                name: "IX_keyitem_keytypenum_keyvaluedate",
                table: "keyitem",
                columns: new[] { "keytypenum", "keyvaluedate" });

            migrationBuilder.CreateIndex(
                name: "IX_keyitem_keytypenum_keyvaluenum",
                table: "keyitem",
                columns: new[] { "keytypenum", "keyvaluenum" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "keyitem");

            migrationBuilder.DropTable(
                name: "itemdata");
        }
    }
}
