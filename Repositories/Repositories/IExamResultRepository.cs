using ExamApp.Repositories.Entities;

namespace ExamApp.Repositories.Repositories
{
    public interface IExamResultRepository : IGenericRepository<ExamResult>
    {
        Task<decimal> GetAverageScoreByExamAsync(int examId);        //genel başarı durumu
        IQueryable<ExamResult> GetByUserId(int userId);
        IQueryable<ExamResult> GetByUserIdAndExamId(int userId, int examId);
    }

}
