﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExamApp.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class addExamSoftDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Exams",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Exams");
        }
    }
}
