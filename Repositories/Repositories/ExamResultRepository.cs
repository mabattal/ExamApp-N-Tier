using ExamApp.Repositories.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExamApp.Repositories.Repositories
{
    public class ExamResultRepository(AppDbContext context) : GenericRepository<ExamResult>(context), IExamResultRepository
    {
        public Task<double> GetAverageScoreByExamAsync(int examId)
        {
            return context.ExamResults.Where(er => er.ExamId == examId).AverageAsync(er => er.Score);
        }

        public IQueryable<ExamResult> GetByUserId(int userId)
        {
            return context.ExamResults.Where(er => er.UserId == userId);
        }
    }
}
