using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExamApp.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class ReorderUserColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Tüm Foreign Key bağlantılarını kaldır
            migrationBuilder.Sql(@"
            DECLARE @sql NVARCHAR(MAX) = '';
            SELECT @sql += 'ALTER TABLE [' + OBJECT_NAME(fk.parent_object_id) + '] DROP CONSTRAINT [' + fk.name + '];'
            FROM sys.foreign_keys fk
            WHERE fk.referenced_object_id = OBJECT_ID('Users');
            EXEC sp_executesql @sql;
        ");

            // Yeni sıralama ile geçici tablo oluştur
            migrationBuilder.CreateTable(
                name: "TempUsers",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    Role = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TempUsers", x => x.UserId);
                });

            // IDENTITY_INSERT aç
            migrationBuilder.Sql("SET IDENTITY_INSERT TempUsers ON;");

            // Verileri taşı
            migrationBuilder.Sql(@"
            INSERT INTO TempUsers (UserId, FullName, Email, Password, Role, IsDeleted) 
            SELECT UserId, FullName, Email, Password, Role, IsDeleted FROM Users;
        ");

            // IDENTITY_INSERT kapat
            migrationBuilder.Sql("SET IDENTITY_INSERT TempUsers OFF;");

            // Eski Users tablosunu sil
            migrationBuilder.DropTable(name: "Users");

            // TempUsers'ı Users olarak yeniden adlandır
            migrationBuilder.RenameTable(name: "TempUsers", newName: "Users");

            // FOREIGN KEY bağlantılarını tekrar ekle
            migrationBuilder.Sql(@"
            DECLARE @sql NVARCHAR(MAX) = '';
            SELECT @sql += 'ALTER TABLE [' + OBJECT_NAME(fkc.parent_object_id) + '] 
                            ADD CONSTRAINT [' + fk.name + '] FOREIGN KEY ([' + pc.name + ']) 
                            REFERENCES Users(UserId);'
            FROM sys.foreign_keys fk
            INNER JOIN sys.foreign_key_columns fkc ON fk.object_id = fkc.constraint_object_id
            INNER JOIN sys.columns pc ON fkc.parent_object_id = pc.object_id AND fkc.parent_column_id = pc.column_id
            WHERE fk.referenced_object_id = OBJECT_ID('Users');
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
            WHERE fk.referenced_object_id = OBJECT_ID('Users');
            EXEC sp_executesql @sql;
        ");

            // Eski Users tablosunu oluştur
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    Role = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    FullName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            // IDENTITY_INSERT aç
            migrationBuilder.Sql("SET IDENTITY_INSERT Users ON;");

            // Verileri geri taşı
            migrationBuilder.Sql(@"
            INSERT INTO Users (UserId, Email, Password, Role, IsDeleted, FullName) 
            SELECT UserId, Email, Password, Role, IsDeleted, FullName FROM Users;
        ");

            // IDENTITY_INSERT kapat
            migrationBuilder.Sql("SET IDENTITY_INSERT Users OFF;");

            // Yeni tabloyu sil
            migrationBuilder.DropTable(name: "Users");

            // Users tablosunu geri yükle
            migrationBuilder.RenameTable(name: "Users", newName: "Users");

            // FOREIGN KEY bağlantılarını geri ekle
            migrationBuilder.Sql(@"
            DECLARE @sql NVARCHAR(MAX) = '';
            SELECT @sql += 'ALTER TABLE [' + OBJECT_NAME(fkc.parent_object_id) + '] 
                            ADD CONSTRAINT [' + fk.name + '] FOREIGN KEY ([' + pc.name + ']) 
                            REFERENCES Users(UserId);'
            FROM sys.foreign_keys fk
            INNER JOIN sys.foreign_key_columns fkc ON fk.object_id = fkc.constraint_object_id
            INNER JOIN sys.columns pc ON fkc.parent_object_id = pc.object_id AND fkc.parent_column_id = pc.column_id
            WHERE fk.referenced_object_id = OBJECT_ID('Users');
            EXEC sp_executesql @sql;
        ");
        }
    }



}
