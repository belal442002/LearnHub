using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnHub.API.Migrations
{
    /// <inheritdoc />
    public partial class AddedMaterialTitle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MaterilaTitle",
                table: "Material",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5873cd1d-8878-41c8-9b4c-130989a7355c",
                columns: new[] { "PasswordHash", "SecurityStamp" },
                values: new object[] { "AQAAAAIAAYagAAAAEANo5m69cTZWju5aJGCNEO9usw3gXUyTh4kR3XiTJmoX8L9QBzAtM/obDRLSJv8Qog==", "be783acb-1618-46b0-a27b-7a115e717d34" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaterilaTitle",
                table: "Material");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5873cd1d-8878-41c8-9b4c-130989a7355c",
                columns: new[] { "PasswordHash", "SecurityStamp" },
                values: new object[] { "AQAAAAIAAYagAAAAENeScJzeHtF5pVPoPDCn3OYhRIV1ZwRo4AkAoOvIHG0AhzeCTbBX7p5/qO20TXl0tQ==", "84111783-e0f5-4f52-a1a9-c870125b03f5" });
        }
    }
}
