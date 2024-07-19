using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Appointment.Migrations
{
    /// <inheritdoc />
    public partial class ChangeAnimalTypeToAnimalTypeId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AnimalTypeId",
                table: "Bookings",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnimalTypeId",
                table: "Bookings");
        }
    }
}
