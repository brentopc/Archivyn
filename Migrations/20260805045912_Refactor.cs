using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Archivyn.Migrations
{
    /// <inheritdoc />
    public partial class Refactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ITEMTYPEGROUP",
                columns: table => new
                {
                    itemtypegroupnum = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    itemtypegroupname = table.Column<string>(type: "character varying(66)", maxLength: 66, nullable: false),
                    flags = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ITEMTYPEGROUP", x => x.itemtypegroupnum);
                });

            migrationBuilder.CreateTable(
                name: "KEYTYPETABLE",
                columns: table => new
                {
                    keytypenum = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    AddToAllDocumentTypes = table.Column<bool>(type: "boolean", nullable: false),
                    IsRequiredOnAllDocumentTypes = table.Column<bool>(type: "boolean", nullable: false),
                    AllDocumentTypesDisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    keytypename = table.Column<string>(type: "character varying(51)", maxLength: 51, nullable: false),
                    datatype = table.Column<int>(type: "integer", nullable: false),
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
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    keysetname = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    iskeytypegroup = table.Column<long>(type: "bigint", nullable: false),
                    flags = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KEYWORDSET", x => x.keysettablenum);
                });

            migrationBuilder.CreateTable(
                name: "DOCTYPE",
                columns: table => new
                {
                    itemtypenum = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    itemtypename = table.Column<string>(type: "character varying(66)", maxLength: 66, nullable: false),
                    itemtypegroupnum = table.Column<long>(type: "bigint", nullable: false),
                    autonamestring = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DOCTYPE", x => x.itemtypenum);
                    table.ForeignKey(
                        name: "FK_DOCTYPE_ITEMTYPEGROUP_itemtypegroupnum",
                        column: x => x.itemtypegroupnum,
                        principalTable: "ITEMTYPEGROUP",
                        principalColumn: "itemtypegroupnum",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "KEYWORDSETKEYTYPE",
                columns: table => new
                {
                    keysettablenum = table.Column<long>(type: "bigint", nullable: false),
                    keytypenum = table.Column<long>(type: "bigint", nullable: false),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
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

            migrationBuilder.CreateTable(
                name: "DOCTYPEKEYTYPE",
                columns: table => new
                {
                    itemtypenum = table.Column<long>(type: "bigint", nullable: false),
                    keytypenum = table.Column<long>(type: "bigint", nullable: false),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsRequired = table.Column<bool>(type: "boolean", nullable: false),
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
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
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

            migrationBuilder.InsertData(
                table: "ITEMTYPEGROUP",
                columns: new[] { "itemtypegroupnum", "IsSystem", "itemtypegroupname" },
                values: new object[] { 1L, true, "System Documents" });

            migrationBuilder.InsertData(
                table: "KEYTYPETABLE",
                columns: new[] { "keytypenum", "AddToAllDocumentTypes", "AllDocumentTypesDisplayOrder", "datatype", "IsRequiredOnAllDocumentTypes", "IsSystem", "keytypelen", "keytypename" },
                values: new object[] { 1L, true, 1, 3, true, true, 10L, ">> Document Date" });

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

            migrationBuilder.CreateIndex(
                name: "IX_DOCTYPE_itemtypegroupnum",
                table: "DOCTYPE",
                column: "itemtypegroupnum");

            migrationBuilder.CreateIndex(
                name: "IX_DOCTYPE_itemtypename",
                table: "DOCTYPE",
                column: "itemtypename",
                unique: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_itemdata_itemtypegroupnum",
                table: "itemdata",
                column: "itemtypegroupnum");

            migrationBuilder.CreateIndex(
                name: "IX_itemdata_itemtypenum",
                table: "itemdata",
                column: "itemtypenum");

            migrationBuilder.CreateIndex(
                name: "IX_ITEMTYPEGROUP_itemtypegroupname",
                table: "ITEMTYPEGROUP",
                column: "itemtypegroupname",
                unique: true);

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
                name: "DOCTYPEKEYTYPE");

            migrationBuilder.DropTable(
                name: "DOCTYPEKEYTYPEGROUP");

            migrationBuilder.DropTable(
                name: "keyitem");

            migrationBuilder.DropTable(
                name: "KEYWORDSETKEYTYPE");

            migrationBuilder.DropTable(
                name: "itemdata");

            migrationBuilder.DropTable(
                name: "KEYTYPETABLE");

            migrationBuilder.DropTable(
                name: "KEYWORDSET");

            migrationBuilder.DropTable(
                name: "DOCTYPE");

            migrationBuilder.DropTable(
                name: "ITEMTYPEGROUP");
        }
    }
}
