using ExamApp.Repositories.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExamApp.Repositories.Repositories
{
    public class ExamRepository(AppDbContext context) : GenericRepository<Exam>(context), IExamRepository
    {
        public IQueryable<Exam> GetByInstructor(int instructorId)
        {
            return context.Exams
                .Where(e => !e.IsDeleted && e.CreatedBy == instructorId)
                .Include(e => e.Questions);
        }

        public IQueryable<Exam> GetActiveExams()
        {
            return context.Exams
                .Where(e => !e.IsDeleted && e.StartDate <= DateTime.Now && e.EndDate >= DateTime.Now)
                .Include(e => e.Instructor);
        }

        public Task<Exam?> GetExamWithDetailsAsync(int examId)
        {
            return context.Exams
                .Where(e => !e.IsDeleted)
                .Include(e => e.Questions)
                .Include(e => e.Instructor)
                .FirstOrDefaultAsync(e => e.ExamId == examId);
        }
    }
}
