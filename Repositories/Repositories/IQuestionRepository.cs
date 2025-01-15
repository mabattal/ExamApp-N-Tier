using ExamApp.Repositories.Entities;

namespace ExamApp.Repositories.Repositories
{
    public interface IQuestionRepository : IGenericRepository<Question>
    {
        IQueryable<Question> GetByExamId(int examId);
    }
}
