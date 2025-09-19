using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EntitySednica.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "pozicija",
                columns: table => new
                {
                    id_pozicije = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    naziv_pozicije = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pozicija", x => x.id_pozicije);
                });

            migrationBuilder.CreateTable(
                name: "saziv",
                columns: table => new
                {
                    id_saziva = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Pocetak = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Kraj = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_saziv", x => x.id_saziva);
                });

            migrationBuilder.CreateTable(
                name: "stranka",
                columns: table => new
                {
                    id_stranke = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    naziv_stranke = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stranka", x => x.id_stranke);
                });

            migrationBuilder.CreateTable(
                name: "tipovi",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tip_zasedanja = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tipovi", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "lica",
                columns: table => new
                {
                    id_lica = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Prezime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    pozicija = table.Column<int>(type: "int", nullable: false),
                    stranka = table.Column<int>(type: "int", nullable: false),
                    Pol = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    datumr = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    korisnicko_ime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Lozinka = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lica", x => x.id_lica);
                    table.ForeignKey(
                        name: "FK_lica_pozicija_pozicija",
                        column: x => x.pozicija,
                        principalTable: "pozicija",
                        principalColumn: "id_pozicije",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_lica_stranka_stranka",
                        column: x => x.stranka,
                        principalTable: "stranka",
                        principalColumn: "id_stranke",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "zasedanje",
                columns: table => new
                {
                    id_zasedanja = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tip = table.Column<int>(type: "int", nullable: false),
                    naziv_zasedanja = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    id_saziv = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_zasedanje", x => x.id_zasedanja);
                    table.ForeignKey(
                        name: "FK_zasedanje_saziv_id_saziv",
                        column: x => x.id_saziv,
                        principalTable: "saziv",
                        principalColumn: "id_saziva",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_zasedanje_tipovi_tip",
                        column: x => x.tip,
                        principalTable: "tipovi",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "mandat",
                columns: table => new
                {
                    id_mandata = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_lica = table.Column<int>(type: "int", nullable: false),
                    id_saziva = table.Column<int>(type: "int", nullable: false),
                    id_stranke = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mandat", x => x.id_mandata);
                    table.ForeignKey(
                        name: "FK_mandat_lica_id_lica",
                        column: x => x.id_lica,
                        principalTable: "lica",
                        principalColumn: "id_lica",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_mandat_saziv_id_saziva",
                        column: x => x.id_saziva,
                        principalTable: "saziv",
                        principalColumn: "id_saziva",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_mandat_stranka_id_stranke",
                        column: x => x.id_stranke,
                        principalTable: "stranka",
                        principalColumn: "id_stranke",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "sednica",
                columns: table => new
                {
                    id_sednice = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Datum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    zasedanje_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sednica", x => x.id_sednice);
                    table.ForeignKey(
                        name: "FK_sednica_zasedanje_zasedanje_id",
                        column: x => x.zasedanje_id,
                        principalTable: "zasedanje",
                        principalColumn: "id_zasedanja",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "dnevni_red",
                columns: table => new
                {
                    id_dnevni_red = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_sednice = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dnevni_red", x => x.id_dnevni_red);
                    table.ForeignKey(
                        name: "FK_dnevni_red_sednica_id_sednice",
                        column: x => x.id_sednice,
                        principalTable: "sednica",
                        principalColumn: "id_sednice",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "pitanja",
                columns: table => new
                {
                    id_pitanja = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_dnevni_red = table.Column<int>(type: "int", nullable: false),
                    redni_broj = table.Column<int>(type: "int", nullable: false),
                    Tekst = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pitanja", x => x.id_pitanja);
                    table.ForeignKey(
                        name: "FK_pitanja_dnevni_red_id_dnevni_red",
                        column: x => x.id_dnevni_red,
                        principalTable: "dnevni_red",
                        principalColumn: "id_dnevni_red",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "glasanje",
                columns: table => new
                {
                    id_glasanja = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_pitanja = table.Column<int>(type: "int", nullable: false),
                    id_lica = table.Column<int>(type: "int", nullable: false),
                    Glas = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_glasanje", x => x.id_glasanja);
                    table.ForeignKey(
                        name: "FK_glasanje_lica_id_lica",
                        column: x => x.id_lica,
                        principalTable: "lica",
                        principalColumn: "id_lica",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_glasanje_pitanja_id_pitanja",
                        column: x => x.id_pitanja,
                        principalTable: "pitanja",
                        principalColumn: "id_pitanja",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_dnevni_red_id_sednice",
                table: "dnevni_red",
                column: "id_sednice");

            migrationBuilder.CreateIndex(
                name: "IX_glasanje_id_lica",
                table: "glasanje",
                column: "id_lica");

            migrationBuilder.CreateIndex(
                name: "IX_glasanje_id_pitanja",
                table: "glasanje",
                column: "id_pitanja");

            migrationBuilder.CreateIndex(
                name: "IX_lica_pozicija",
                table: "lica",
                column: "pozicija");

            migrationBuilder.CreateIndex(
                name: "IX_lica_stranka",
                table: "lica",
                column: "stranka");

            migrationBuilder.CreateIndex(
                name: "IX_mandat_id_lica",
                table: "mandat",
                column: "id_lica");

            migrationBuilder.CreateIndex(
                name: "IX_mandat_id_saziva",
                table: "mandat",
                column: "id_saziva");

            migrationBuilder.CreateIndex(
                name: "IX_mandat_id_stranke",
                table: "mandat",
                column: "id_stranke");

            migrationBuilder.CreateIndex(
                name: "IX_pitanja_id_dnevni_red",
                table: "pitanja",
                column: "id_dnevni_red");

            migrationBuilder.CreateIndex(
                name: "IX_sednica_zasedanje_id",
                table: "sednica",
                column: "zasedanje_id");

            migrationBuilder.CreateIndex(
                name: "IX_zasedanje_id_saziv",
                table: "zasedanje",
                column: "id_saziv");

            migrationBuilder.CreateIndex(
                name: "IX_zasedanje_tip",
                table: "zasedanje",
                column: "tip");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "glasanje");

            migrationBuilder.DropTable(
                name: "mandat");

            migrationBuilder.DropTable(
                name: "pitanja");

            migrationBuilder.DropTable(
                name: "lica");

            migrationBuilder.DropTable(
                name: "dnevni_red");

            migrationBuilder.DropTable(
                name: "pozicija");

            migrationBuilder.DropTable(
                name: "stranka");

            migrationBuilder.DropTable(
                name: "sednica");

            migrationBuilder.DropTable(
                name: "zasedanje");

            migrationBuilder.DropTable(
                name: "saziv");

            migrationBuilder.DropTable(
                name: "tipovi");
        }
    }
}
