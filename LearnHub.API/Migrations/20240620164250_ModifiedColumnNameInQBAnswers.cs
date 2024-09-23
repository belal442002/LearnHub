using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnHub.API.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedColumnNameInQBAnswers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QuestionText",
                table: "QBAnswers",
                newName: "AnswerText");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5873cd1d-8878-41c8-9b4c-130989a7355c",
                columns: new[] { "PasswordHash", "SecurityStamp" },
                values: new object[] { "AQAAAAIAAYagAAAAEIv7eEny1NzqmidYXNH7EHEIwCXQp3v1INzov1U3UZwnB4DC43Fx1XG6ulN/AMewgg==", "2e00b729-b54e-44b6-8b1e-33b2b55b0763" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AnswerText",
                table: "QBAnswers",
                newName: "QuestionText");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5873cd1d-8878-41c8-9b4c-130989a7355c",
                columns: new[] { "PasswordHash", "SecurityStamp" },
                values: new object[] { "AQAAAAIAAYagAAAAED43E6HjGRFrhHwRkpHjEcsy4OHFGwovUCRR6bTEXi+sT7+iUQMWVgx4IqvyU9D9kQ==", "95df3fed-fd9b-48ec-8f51-6071b8c72912" });
        }
    }
}
