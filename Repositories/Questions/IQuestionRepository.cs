namespace ExamApp.Repositories.Questions
{
    public interface IQuestionRepository : IGenericRepository<Question>
    {
        IQueryable<Question> GetByExamId(int examId);
    }
}
