using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnHub.API.Migrations
{
    /// <inheritdoc />
    public partial class AddedParentRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccountId",
                table: "Parent",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "3125b3a6-1b98-4d83-b473-8a6f4dd73f53", "3125b3a6-1b98-4d83-b473-8a6f4dd73f53", "Parent", "PARENT" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5873cd1d-8878-41c8-9b4c-130989a7355c",
                columns: new[] { "PasswordHash", "SecurityStamp" },
                values: new object[] { "AQAAAAIAAYagAAAAENeScJzeHtF5pVPoPDCn3OYhRIV1ZwRo4AkAoOvIHG0AhzeCTbBX7p5/qO20TXl0tQ==", "84111783-e0f5-4f52-a1a9-c870125b03f5" });

            migrationBuilder.CreateIndex(
                name: "IX_Parent_AccountId",
                table: "Parent",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Parent_AspNetUsers_AccountId",
                table: "Parent",
                column: "AccountId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Parent_AspNetUsers_AccountId",
                table: "Parent");

            migrationBuilder.DropIndex(
                name: "IX_Parent_AccountId",
                table: "Parent");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3125b3a6-1b98-4d83-b473-8a6f4dd73f53");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Parent");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5873cd1d-8878-41c8-9b4c-130989a7355c",
                columns: new[] { "PasswordHash", "SecurityStamp" },
                values: new object[] { "AQAAAAIAAYagAAAAEEQjADYtkniY3dF/wxreuMoGbZn9RlSCdOfC91T3CFO0CwqsUIVhbte40soPOjuxGA==", "ea3e1d14-c027-47a0-94d9-dece1e8b5415" });
        }
    }
}
