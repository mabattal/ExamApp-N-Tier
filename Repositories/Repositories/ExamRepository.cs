using ExamApp.Repositories.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExamApp.Repositories.Repositories
{
    public class ExamRepository(AppDbContext context) : GenericRepository<Exam>(context), IExamRepository
    {
        public IQueryable<Exam> GetByInstructor(int instructorId)
        {
            return context.Exams.Where(e => e.CreatedBy == instructorId);
        }

        public IQueryable<Exam> GetActiveExams()
        {
            return context.Exams.Where(e => e.StartDate <= DateTime.UtcNow && e.EndDate >= DateTime.UtcNow);
        }

        public Task<Exam?> GetExamWithDetailsAsync(int examId)
        {
            return context.Exams.Include(e => e.Questions)
                .Include(e => e.Instructor)
                .FirstOrDefaultAsync(e => e.ExamId == examId);
        }
    }
}
