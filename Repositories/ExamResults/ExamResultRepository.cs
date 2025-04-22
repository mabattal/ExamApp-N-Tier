using ExamApp.Repositories.Database;
using Microsoft.EntityFrameworkCore;

namespace ExamApp.Repositories.ExamResults
{
    public class ExamResultRepository(AppDbContext context) : GenericRepository<ExamResult>(context), IExamResultRepository
    {
        public Task<decimal> GetAverageScoreByExamAsync(int examId)
        {
            return context.ExamResults.Where(er => er.ExamId == examId && er.Score != null).AverageAsync(er => (decimal)er.Score!);
        }

        public Task<decimal> GetMaxScoreByExamAsync(int examId)
        {
            return context.ExamResults.Where(er => er.ExamId == examId && er.Score != null).MaxAsync(er => (decimal)er.Score!);
        }

        public Task<decimal> GetMinScoreByExamAsync(int examId)
        {
            return context.ExamResults.Where(er => er.ExamId == examId && er.Score != null).MinAsync(er => (decimal)er.Score!);
        }

        public Task<int> GetExamCountByExamAsync(int examId)
        {
            return context.ExamResults.Where(er => er.ExamId == examId).CountAsync();
        }

        public IQueryable<ExamResult> GetByUserId(int userId)
        {
            return context.ExamResults.Where(er => er.UserId == userId);
        }

        public IQueryable<ExamResult> GetByExamId(int examId)
        {
            return context.ExamResults.Include(u => u.User).Where(er => er.ExamId == examId);
        }

        public IQueryable<ExamResult> GetByUserIdAndExamId(int userId, int examId)
        {
            return context.ExamResults.Where(er => er.UserId == userId && er.ExamId == examId);
        }
    }
}
