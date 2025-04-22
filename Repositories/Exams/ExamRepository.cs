using ExamApp.Repositories.Database;
using Microsoft.EntityFrameworkCore;

namespace ExamApp.Repositories.Exams
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
                .Where(e => !e.IsDeleted && e.StartDate <= DateTimeOffset.UtcNow && e.EndDate >= DateTimeOffset.UtcNow && e.Questions.Count > 0)
                .Include(e => e.Instructor);
        }
        public IQueryable<Exam> GetPastExams()
        {
            return context.Exams
                .Where(e => !e.IsDeleted && e.EndDate <= DateTimeOffset.UtcNow && e.Questions.Count > 0)
                .Include(e => e.Instructor);
        }

        public IQueryable<Exam> GetUpcomingExams()
        {
            return context.Exams
                .Where(e => !e.IsDeleted && e.StartDate >= DateTimeOffset.UtcNow && e.Questions.Count > 0)
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
