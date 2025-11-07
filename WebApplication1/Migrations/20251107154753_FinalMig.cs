using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebApplication1.Migrations
{
    public partial class FinalMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Stats",
                keyColumn: "id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Stats",
                keyColumn: "id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Stats",
                keyColumn: "id",
                keyValue: 3L);

            migrationBuilder.RenameColumn(
                name: "usersAmmount",
                table: "Stats",
                newName: "visitsCount");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "Stats",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "dau",
                table: "Stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "mau",
                table: "Stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "newUsers",
                table: "Stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "newUsersBounce",
                table: "Stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "newUsersNoBounce",
                table: "Stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "returningUsers",
                table: "Stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "returningUsersBounce",
                table: "Stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "returningUsersNoBounce",
                table: "Stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "trafficSource",
                table: "Stats",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "dau",
                table: "Stats");

            migrationBuilder.DropColumn(
                name: "mau",
                table: "Stats");

            migrationBuilder.DropColumn(
                name: "newUsers",
                table: "Stats");

            migrationBuilder.DropColumn(
                name: "newUsersBounce",
                table: "Stats");

            migrationBuilder.DropColumn(
                name: "newUsersNoBounce",
                table: "Stats");

            migrationBuilder.DropColumn(
                name: "returningUsers",
                table: "Stats");

            migrationBuilder.DropColumn(
                name: "returningUsersBounce",
                table: "Stats");

            migrationBuilder.DropColumn(
                name: "returningUsersNoBounce",
                table: "Stats");

            migrationBuilder.DropColumn(
                name: "trafficSource",
                table: "Stats");

            migrationBuilder.RenameColumn(
                name: "visitsCount",
                table: "Stats",
                newName: "usersAmmount");

            migrationBuilder.AlterColumn<long>(
                name: "id",
                table: "Stats",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

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
    }
}
