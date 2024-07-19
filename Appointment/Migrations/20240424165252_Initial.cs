using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Appointment.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AppointmentDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ClientEmail = table.Column<string>(type: "text", nullable: true),
                    DoctorId = table.Column<int>(type: "integer", nullable: true),
                    VetAidId = table.Column<int>(type: "integer", nullable: true),
                    PetId = table.Column<int>(type: "integer", nullable: true),
                    AppointmentStatus = table.Column<int>(type: "integer", nullable: false),
                    BookingNode = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookings");
        }
    }
}
