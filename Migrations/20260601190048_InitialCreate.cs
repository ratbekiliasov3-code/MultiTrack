using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MultiTrack.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Harcamalar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Tarih = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Aciklama = table.Column<string>(type: "TEXT", nullable: false),
                    Tutar = table.Column<double>(type: "REAL", nullable: false),
                    KullaniciId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Harcamalar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Kitaplar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    KitapAdi = table.Column<string>(type: "TEXT", nullable: false),
                    ToplamSayfa = table.Column<int>(type: "INTEGER", nullable: false),
                    KalinanSayfa = table.Column<int>(type: "INTEGER", nullable: false),
                    KullaniciId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kitaplar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Kullanici",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kullanici", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sporlar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Gun = table.Column<string>(type: "TEXT", nullable: false),
                    AntrenmanAdi = table.Column<string>(type: "TEXT", nullable: false),
                    KullaniciId = table.Column<string>(type: "TEXT", nullable: false),
                    IsCompleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sporlar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sular",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Tarih = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Miktar = table.Column<double>(type: "REAL", nullable: false),
                    KullaniciId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sular", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Gorevler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Baslik = table.Column<string>(type: "TEXT", nullable: false),
                    Tarih = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsCompleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    KullaniciId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gorevler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Gorevler_Kullanici_KullaniciId",
                        column: x => x.KullaniciId,
                        principalTable: "Kullanici",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Gorevler_KullaniciId",
                table: "Gorevler",
                column: "KullaniciId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Gorevler");

            migrationBuilder.DropTable(
                name: "Harcamalar");

            migrationBuilder.DropTable(
                name: "Kitaplar");

            migrationBuilder.DropTable(
                name: "Sporlar");

            migrationBuilder.DropTable(
                name: "Sular");

            migrationBuilder.DropTable(
                name: "Kullanici");
        }
    }
}
