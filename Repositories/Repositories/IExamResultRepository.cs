using ExamApp.Repositories.Entities;

namespace ExamApp.Repositories.Repositories
{
    public interface IExamResultRepository : IGenericRepository<ExamResult>
    {
        Task<double> GetAverageScoreByExamAsync(int examId);        //genel başarı durumu
        IQueryable<ExamResult> GetByUserId(int userId);
    }

}
