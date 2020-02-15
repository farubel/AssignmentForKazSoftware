using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UrlShortenProject.Migrations
{
    public partial class InitialDatabaseCreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UrlShortens",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LongUrl = table.Column<string>(maxLength: 200, nullable: false),
                    ShortenedUrl = table.Column<string>(maxLength: 30, nullable: false),
                    Token = table.Column<string>(maxLength: 10, nullable: false),
                    Created = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrlShortens", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UrlShortens");
        }
    }
}
