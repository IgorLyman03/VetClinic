using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoctorProfile.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUserIdUniqueConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DoctorTimetables_UserId",
                table: "DoctorTimetables");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorTimetables_UserId",
                table: "DoctorTimetables",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DoctorTimetables_UserId",
                table: "DoctorTimetables");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorTimetables_UserId",
                table: "DoctorTimetables",
                column: "UserId",
                unique: true);
        }
    }
}
