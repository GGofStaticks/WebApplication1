using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebApplication1.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Stats",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    usersAmmount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stats", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "Stats",
                columns: new[] { "id", "date", "usersAmmount" },
                values: new object[,]
                {
                    { 1L, new DateTime(2025, 9, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 37 },
                    { 2L, new DateTime(2025, 9, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), 41 },
                    { 3L, new DateTime(2025, 9, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), 24 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Stats");
        }
    }
}
