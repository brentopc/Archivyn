using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Archivyn.Migrations
{
    /// <inheritdoc />
    public partial class AddDocumentTypes : Migration
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
                    itemtypegroupname = table.Column<string>(type: "character varying(66)", maxLength: 66, nullable: false),
                    flags = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ITEMTYPEGROUP", x => x.itemtypegroupnum);
                });

            migrationBuilder.CreateTable(
                name: "DOCTYPE",
                columns: table => new
                {
                    itemtypenum = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
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
                name: "IX_ITEMTYPEGROUP_itemtypegroupname",
                table: "ITEMTYPEGROUP",
                column: "itemtypegroupname",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DOCTYPE");

            migrationBuilder.DropTable(
                name: "ITEMTYPEGROUP");
        }
    }
}
