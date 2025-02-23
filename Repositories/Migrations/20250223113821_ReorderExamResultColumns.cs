using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExamApp.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class ReorderExamResultColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Tüm Foreign Key bağlantılarını kaldır
            migrationBuilder.Sql(@"
            DECLARE @sql NVARCHAR(MAX) = '';
            SELECT @sql += 'ALTER TABLE [' + OBJECT_NAME(fk.parent_object_id) + '] DROP CONSTRAINT [' + fk.name + '];'
            FROM sys.foreign_keys fk
            WHERE fk.referenced_object_id = OBJECT_ID('ExamResult');
            EXEC sp_executesql @sql;
        ");

            // Yeni sıralama ile geçici tablo oluştur
            migrationBuilder.CreateTable(
                name: "TempExamResult",
                columns: table => new
                {
                    ResultId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    ExamId = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    CompletionDate = table.Column<DateTime>(nullable: true),
                    Duration = table.Column<int>(nullable: true),
                    TotalQuestions = table.Column<int>(nullable: false),
                    CorrectAnswers = table.Column<int>(nullable: true),
                    IncorrectAnswers = table.Column<int>(nullable: true),
                    EmptyAnswers = table.Column<int>(nullable: true),
                    Score = table.Column<decimal>(type: "decimal(10,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TempExamResult", x => x.ResultId);
                });

            // IDENTITY_INSERT aç
            migrationBuilder.Sql("SET IDENTITY_INSERT TempExamResult ON;");

            // Verileri taşı
            migrationBuilder.Sql(@"
            INSERT INTO TempExamResult (ResultId, UserId, ExamId, StartDate, CompletionDate, Duration, TotalQuestions, CorrectAnswers, IncorrectAnswers, EmptyAnswers, Score) 
            SELECT ResultId, UserId, ExamId, StartDate, CompletionDate, Duration, TotalQuestions, CorrectAnswers, IncorrectAnswers, EmptyAnswers, Score FROM ExamResults;
        ");

            // IDENTITY_INSERT kapat
            migrationBuilder.Sql("SET IDENTITY_INSERT TempExamResult OFF;");

            // Eski ExamResult tablosunu sil
            migrationBuilder.DropTable(name: "ExamResults");

            // TempExamResult'ı ExamResult olarak yeniden adlandır
            migrationBuilder.RenameTable(name: "TempExamResult", newName: "ExamResults");

            // FOREIGN KEY bağlantılarını tekrar ekle
            migrationBuilder.Sql(@"
            DECLARE @sql NVARCHAR(MAX) = '';
            SELECT @sql += 'ALTER TABLE [' + OBJECT_NAME(fkc.parent_object_id) + '] 
                            ADD CONSTRAINT [' + fk.name + '] FOREIGN KEY ([' + pc.name + ']) 
                            REFERENCES ExamResults(ResultId);'
            FROM sys.foreign_keys fk
            INNER JOIN sys.foreign_key_columns fkc ON fk.object_id = fkc.constraint_object_id
            INNER JOIN sys.columns pc ON fkc.parent_object_id = pc.object_id AND fkc.parent_column_id = pc.column_id
            WHERE fk.referenced_object_id = OBJECT_ID('ExamResults');
            EXEC sp_executesql @sql;
        ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // FOREIGN KEY bağlantılarını kaldır
            migrationBuilder.Sql(@"
            DECLARE @sql NVARCHAR(MAX) = '';
            SELECT @sql += 'ALTER TABLE [' + OBJECT_NAME(fk.parent_object_id) + '] DROP CONSTRAINT [' + fk.name + '];'
            FROM sys.foreign_keys fk
            WHERE fk.referenced_object_id = OBJECT_ID('ExamResults');
            EXEC sp_executesql @sql;
        ");

            // Eski ExamResult tablosunu oluştur
            migrationBuilder.CreateTable(
                name: "ExamResults",
                columns: table => new
                {
                    ResultId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    ExamId = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    CompletionDate = table.Column<DateTime>(nullable: true),
                    Duration = table.Column<int>(nullable: true),
                    TotalQuestions = table.Column<int>(nullable: false),
                    CorrectAnswers = table.Column<int>(nullable: true),
                    IncorrectAnswers = table.Column<int>(nullable: true),
                    EmptyAnswers = table.Column<int>(nullable: true),
                    Score = table.Column<decimal>(type: "decimal(10,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamResults", x => x.ResultId);
                });

            // IDENTITY_INSERT aç
            migrationBuilder.Sql("SET IDENTITY_INSERT ExamResults ON;");

            // Verileri geri taşı
            migrationBuilder.Sql(@"
            INSERT INTO ExamResults (ResultId, UserId, ExamId, StartDate, CompletionDate, Duration, TotalQuestions, CorrectAnswers, IncorrectAnswers, EmptyAnswers, Score) 
            SELECT ResultId, UserId, ExamId, StartDate, CompletionDate, Duration, TotalQuestions, CorrectAnswers, IncorrectAnswers, EmptyAnswers, Score FROM ExamResults;
        ");

            // IDENTITY_INSERT kapat
            migrationBuilder.Sql("SET IDENTITY_INSERT ExamResults OFF;");

            // Yeni tabloyu sil
            migrationBuilder.DropTable(name: "ExamResults");

            // ExamResults tablosunu geri yükle
            migrationBuilder.RenameTable(name: "ExamResults", newName: "ExamResults");

            // FOREIGN KEY bağlantılarını geri ekle
            migrationBuilder.Sql(@"
            DECLARE @sql NVARCHAR(MAX) = '';
            SELECT @sql += 'ALTER TABLE [' + OBJECT_NAME(fkc.parent_object_id) + '] 
                            ADD CONSTRAINT [' + fk.name + '] FOREIGN KEY ([' + pc.name + ']) 
                            REFERENCES ExamResults(ResultId);'
            FROM sys.foreign_keys fk
            INNER JOIN sys.foreign_key_columns fkc ON fk.object_id = fkc.constraint_object_id
            INNER JOIN sys.columns pc ON fkc.parent_object_id = pc.object_id AND fkc.parent_column_id = pc.column_id
            WHERE fk.referenced_object_id = OBJECT_ID('ExamResults');
            EXEC sp_executesql @sql;
        ");
        }
    }


}
