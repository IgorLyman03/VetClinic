using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VetAid.Migrations
{
    public partial class AddVetAidAnymalTypeManyToMany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VetAids_AnimalTypes_AnimalTypeId",
                table: "VetAids");

            migrationBuilder.DropIndex(
                name: "IX_VetAids_AnimalTypeId",
                table: "VetAids");

            migrationBuilder.DropColumn(
                name: "AnimalTypeId",
                table: "VetAids");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Duretion",
                table: "VetAids",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.CreateTable(
                name: "VetAidAnimalType",
                columns: table => new
                {
                    VetAidId = table.Column<int>(type: "integer", nullable: false),
                    AnimalTypeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VetAidAnimalType", x => new { x.VetAidId, x.AnimalTypeId });
                    table.ForeignKey(
                        name: "FK_VetAidAnimalType_AnimalTypes_VetAidId",
                        column: x => x.VetAidId,
                        principalTable: "AnimalTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VetAidAnimalType_VetAids_AnimalTypeId",
                        column: x => x.AnimalTypeId,
                        principalTable: "VetAids",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VetAidAnimalType_AnimalTypeId",
                table: "VetAidAnimalType",
                column: "AnimalTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VetAidAnimalType");

            migrationBuilder.DropColumn(
                name: "Duretion",
                table: "VetAids");

            migrationBuilder.AddColumn<int>(
                name: "AnimalTypeId",
                table: "VetAids",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_VetAids_AnimalTypeId",
                table: "VetAids",
                column: "AnimalTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_VetAids_AnimalTypes_AnimalTypeId",
                table: "VetAids",
                column: "AnimalTypeId",
                principalTable: "AnimalTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
