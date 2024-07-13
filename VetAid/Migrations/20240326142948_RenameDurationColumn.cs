using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VetAid.Migrations
{
    /// <inheritdoc />
    public partial class RenameDurationColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duretion",
                table: "VetAids");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "VetAids",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Duration",
                table: "VetAids",
                type: "interval",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "VetAids");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "VetAids",
                type: "numeric",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Duretion",
                table: "VetAids",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }
    }
}
