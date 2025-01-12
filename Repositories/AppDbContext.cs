using ExamApp.Repositories.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExamApp.Repositories
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Answer> Answers { get; set; } = null!;
        public DbSet<Exam> Exams { get; set; } = null!;
        public DbSet<Question> Questions { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<ExamResult> ExamResults { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
