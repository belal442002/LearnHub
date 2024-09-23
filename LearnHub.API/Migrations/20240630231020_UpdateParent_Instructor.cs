using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnHub.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateParent_Instructor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Parent",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "Parent",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Instructor",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "Instructor",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5873cd1d-8878-41c8-9b4c-130989a7355c",
                columns: new[] { "PasswordHash", "SecurityStamp" },
                values: new object[] { "AQAAAAIAAYagAAAAEBS8pBezsgFTDRum8reQ3mmNnhVhQJDbfguiabbpBZAudQtgnMpbvmwTJdq9G5vecg==", "cf91fff2-dda1-44af-b558-7c738320ca95" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Parent");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Parent");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Instructor");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Instructor");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5873cd1d-8878-41c8-9b4c-130989a7355c",
                columns: new[] { "PasswordHash", "SecurityStamp" },
                values: new object[] { "AQAAAAIAAYagAAAAEISXSdC+A78r1LJ902JCmO3BfNTtqYUDiDKnfWT9JXyPaJYxC3IDPAFPhOj4p31XGQ==", "f61531be-de6e-4d90-b010-f172d9a15ebf" });
        }
    }
}
