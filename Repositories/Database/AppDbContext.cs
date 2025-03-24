using System.Reflection;
using ExamApp.Repositories.Answers;
using ExamApp.Repositories.ExamResults;
using ExamApp.Repositories.Exams;
using ExamApp.Repositories.Questions;
using ExamApp.Repositories.Users;
using Microsoft.EntityFrameworkCore;

namespace ExamApp.Repositories.Database
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
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}
