using ExamApp.Repositories.Entities;

namespace ExamApp.Repositories.Repositories
{
    public interface IAnswerRepository : IGenericRepository<Answer>
    {
        IQueryable<Answer> GetByUserAndExam(int userId, int examId);
        Task<(int Correct, int Incorrect)> GetAnswerStatisticsAsync(int examId, int userId);
    }
}
