using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NaviatePage.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "customer",
                columns: table => new
                {
                    idcustomer = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    displayname = table.Column<string>(type: "text", nullable: true),
                    address = table.Column<string>(type: "text", nullable: true),
                    phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    moreinfo = table.Column<string>(type: "text", nullable: true),
                    contractdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_customer", x => x.idcustomer);
                });

            migrationBuilder.CreateTable(
                name: "input",
                columns: table => new
                {
                    idinput = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    dateinput = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_input", x => x.idinput);
                });

            migrationBuilder.CreateTable(
                name: "output",
                columns: table => new
                {
                    idoutput = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    dateoutput = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_output", x => x.idoutput);
                });

            migrationBuilder.CreateTable(
                name: "supplier",
                columns: table => new
                {
                    idsupplier = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    displayname = table.Column<string>(type: "text", nullable: true),
                    address = table.Column<string>(type: "text", nullable: true),
                    phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    moreinfo = table.Column<string>(type: "text", nullable: true),
                    contractdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_supplier", x => x.idsupplier);
                });

            migrationBuilder.CreateTable(
                name: "unit",
                columns: table => new
                {
                    idunit = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    displayname = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_unit", x => x.idunit);
                });

            migrationBuilder.CreateTable(
                name: "userrole",
                columns: table => new
                {
                    iduserrole = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    displayname = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_userrole", x => x.iduserrole);
                });

            migrationBuilder.CreateTable(
                name: "material",
                columns: table => new
                {
                    idmaterial = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    displayname = table.Column<string>(type: "text", nullable: true),
                    idunit = table.Column<int>(type: "integer", nullable: false),
                    idsupplier = table.Column<int>(type: "integer", nullable: false),
                    qrcode = table.Column<string>(type: "text", nullable: true),
                    barcode = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_material", x => x.idmaterial);
                    table.ForeignKey(
                        name: "material_idsupplier_fkey",
                        column: x => x.idsupplier,
                        principalTable: "supplier",
                        principalColumn: "idsupplier",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "material_idunit_fkey",
                        column: x => x.idunit,
                        principalTable: "unit",
                        principalColumn: "idunit",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    iduser = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    displayname = table.Column<string>(type: "text", nullable: true),
                    username = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    password = table.Column<string>(type: "text", nullable: true),
                    iduserrole = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.iduser);
                    table.ForeignKey(
                        name: "users_iduserrole_fkey",
                        column: x => x.iduserrole,
                        principalTable: "userrole",
                        principalColumn: "iduserrole");
                });

            migrationBuilder.CreateTable(
                name: "inputinfo",
                columns: table => new
                {
                    idinputinfo = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    idmaterial = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    idinput = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    count = table.Column<int>(type: "integer", nullable: true),
                    inputprice = table.Column<double>(type: "double precision", nullable: true, defaultValueSql: "0"),
                    outputprice = table.Column<double>(type: "double precision", nullable: true, defaultValueSql: "0"),
                    status = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_inputinfo", x => x.idinputinfo);
                    table.ForeignKey(
                        name: "inputinfo_idinput_fkey",
                        column: x => x.idinput,
                        principalTable: "input",
                        principalColumn: "idinput",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "inputinfo_idmaterial_fkey",
                        column: x => x.idmaterial,
                        principalTable: "material",
                        principalColumn: "idmaterial",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "outputinfo",
                columns: table => new
                {
                    idoutputinfo = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    idmaterial = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    idoutput = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    idcustomer = table.Column<int>(type: "integer", nullable: false),
                    count = table.Column<int>(type: "integer", nullable: true),
                    status = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_outputinfo", x => x.idoutputinfo);
                    table.ForeignKey(
                        name: "outputinfo_idcustomer_fkey",
                        column: x => x.idcustomer,
                        principalTable: "customer",
                        principalColumn: "idcustomer",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "outputinfo_idmaterial_fkey",
                        column: x => x.idmaterial,
                        principalTable: "material",
                        principalColumn: "idmaterial",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "outputinfo_idoutput_fkey",
                        column: x => x.idoutput,
                        principalTable: "output",
                        principalColumn: "idoutput",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_inputinfo_idinput",
                table: "inputinfo",
                column: "idinput");

            migrationBuilder.CreateIndex(
                name: "IX_inputinfo_idmaterial",
                table: "inputinfo",
                column: "idmaterial");

            migrationBuilder.CreateIndex(
                name: "IX_material_idsupplier",
                table: "material",
                column: "idsupplier");

            migrationBuilder.CreateIndex(
                name: "IX_material_idunit",
                table: "material",
                column: "idunit");

            migrationBuilder.CreateIndex(
                name: "IX_outputinfo_idcustomer",
                table: "outputinfo",
                column: "idcustomer");

            migrationBuilder.CreateIndex(
                name: "IX_outputinfo_idmaterial",
                table: "outputinfo",
                column: "idmaterial");

            migrationBuilder.CreateIndex(
                name: "IX_outputinfo_idoutput",
                table: "outputinfo",
                column: "idoutput");

            migrationBuilder.CreateIndex(
                name: "IX_users_iduserrole",
                table: "users",
                column: "iduserrole");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "inputinfo");

            migrationBuilder.DropTable(
                name: "outputinfo");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "input");

            migrationBuilder.DropTable(
                name: "customer");

            migrationBuilder.DropTable(
                name: "material");

            migrationBuilder.DropTable(
                name: "output");

            migrationBuilder.DropTable(
                name: "userrole");

            migrationBuilder.DropTable(
                name: "supplier");

            migrationBuilder.DropTable(
                name: "unit");
        }
    }
}
