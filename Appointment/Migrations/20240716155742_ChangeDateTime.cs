using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Appointment.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDateTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "PetId",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "AppointmentStatus",
                table: "Bookings",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "AppointmentDateTime",
                table: "Bookings",
                newName: "StartDate");

            migrationBuilder.AlterColumn<string>(
                name: "DoctorId",
                table: "Bookings",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "EndDate",
                table: "Bookings",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Bookings",
                newName: "AppointmentStatus");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Bookings",
                newName: "AppointmentDateTime");

            migrationBuilder.AlterColumn<int>(
                name: "DoctorId",
                table: "Bookings",
                type: "integer",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

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

            migrationBuilder.AddColumn<int>(
                name: "PetId",
                table: "Bookings",
                type: "integer",
                nullable: true);
        }
    }
}
