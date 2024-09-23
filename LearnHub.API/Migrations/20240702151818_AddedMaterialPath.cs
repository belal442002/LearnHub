using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnHub.API.Migrations
{
    /// <inheritdoc />
    public partial class AddedMaterialPath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MaterialPath",
                table: "Material",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5873cd1d-8878-41c8-9b4c-130989a7355c",
                columns: new[] { "PasswordHash", "SecurityStamp" },
                values: new object[] { "AQAAAAIAAYagAAAAECY8NVpfR6tbGf9SHZNUje3NwkohudMPSF2rwyZzw3ZAqojqcOASs0jheuXpzsVc5A==", "4216031d-7b21-41c0-86b9-22c338bd37b2" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaterialPath",
                table: "Material");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5873cd1d-8878-41c8-9b4c-130989a7355c",
                columns: new[] { "PasswordHash", "SecurityStamp" },
                values: new object[] { "AQAAAAIAAYagAAAAEBS8pBezsgFTDRum8reQ3mmNnhVhQJDbfguiabbpBZAudQtgnMpbvmwTJdq9G5vecg==", "cf91fff2-dda1-44af-b558-7c738320ca95" });
        }
    }
}
