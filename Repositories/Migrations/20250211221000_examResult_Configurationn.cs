using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExamApp.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class examResult_Configurationn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exams_Users_CreatedBy",
                table: "Exams");

            migrationBuilder.AlterColumn<double>(
                name: "Score",
                table: "ExamResults",
                type: "float",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_Users_CreatedBy",
                table: "Exams",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exams_Users_CreatedBy",
                table: "Exams");

            migrationBuilder.AlterColumn<decimal>(
                name: "Score",
                table: "ExamResults",
                type: "decimal(10,2)",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_Users_CreatedBy",
                table: "Exams",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "UserId");
        }
    }
}
