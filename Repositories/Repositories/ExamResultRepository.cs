using ExamApp.Repositories.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExamApp.Repositories.Repositories
{
    public class ExamResultRepository(AppDbContext context) : GenericRepository<ExamResult>(context), IExamResultRepository
    {
        public Task<decimal> GetAverageScoreByExamAsync(int examId)
        {
            return context.ExamResults.Where(er => er.ExamId == examId && er.Score != null).AverageAsync(er => (decimal)er.Score);
        }

        public IQueryable<ExamResult> GetByUserId(int userId)
        {
            return context.ExamResults.Where(er => er.UserId == userId);
        }

        public IQueryable<ExamResult> GetByUserIdAndExamId(int userId, int examId)
        {
            return context.ExamResults.Where(er => er.UserId == userId && er.ExamId == examId);
        }
    }
}
