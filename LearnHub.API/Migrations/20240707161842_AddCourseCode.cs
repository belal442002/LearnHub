using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnHub.API.Migrations
{
    /// <inheritdoc />
    public partial class AddCourseCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CourseCode",
                table: "Course",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5873cd1d-8878-41c8-9b4c-130989a7355c",
                columns: new[] { "PasswordHash", "SecurityStamp" },
                values: new object[] { "AQAAAAIAAYagAAAAEObqELyw7jiC4pypQlki1bc5+O6VcIWkr2OD2TmsKfn9bxEt2msVVC9QE5uD+PQmsw==", "bbc44ba4-1b78-4071-8140-3c7acbce8b64" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourseCode",
                table: "Course");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5873cd1d-8878-41c8-9b4c-130989a7355c",
                columns: new[] { "PasswordHash", "SecurityStamp" },
                values: new object[] { "AQAAAAIAAYagAAAAECY8NVpfR6tbGf9SHZNUje3NwkohudMPSF2rwyZzw3ZAqojqcOASs0jheuXpzsVc5A==", "4216031d-7b21-41c0-86b9-22c338bd37b2" });
        }
    }
}
