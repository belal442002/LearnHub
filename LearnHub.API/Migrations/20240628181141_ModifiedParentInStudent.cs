using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnHub.API.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedParentInStudent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5873cd1d-8878-41c8-9b4c-130989a7355c",
                columns: new[] { "PasswordHash", "SecurityStamp" },
                values: new object[] { "AQAAAAIAAYagAAAAEOCNRyLbWwikNMIuHpdX1NVlfNoY9nZPtbMXOz6DcFbXsZc5vFYiZ2PVhSfE2Ny75Q==", "b204e6f0-5669-4f72-bc30-a7ad1e344301" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5873cd1d-8878-41c8-9b4c-130989a7355c",
                columns: new[] { "PasswordHash", "SecurityStamp" },
                values: new object[] { "AQAAAAIAAYagAAAAEANo5m69cTZWju5aJGCNEO9usw3gXUyTh4kR3XiTJmoX8L9QBzAtM/obDRLSJv8Qog==", "be783acb-1618-46b0-a27b-7a115e717d34" });
        }
    }
}
