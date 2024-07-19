using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Appointment.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldsInBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AnimalTypeId",
                table: "Bookings",
                newName: "PetType");

            migrationBuilder.AddColumn<string>(
                name: "OwnerEmail",
                table: "Bookings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "Bookings",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerTempId",
                table: "Bookings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PetBreed",
                table: "Bookings",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerEmail",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "OwnerTempId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "PetBreed",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "PetType",
                table: "Bookings",
                newName: "AnimalTypeId");
        }
    }
}
