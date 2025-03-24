using ExamApp.Repositories.Database;

namespace ExamApp.Repositories.Questions
{
    public class QuestionRepository(AppDbContext context) : GenericRepository<Question>(context), IQuestionRepository
    {
        public IQueryable<Question> GetByExamId(int examId)
        {
            return context.Questions.Where(q => q.ExamId == examId);
        }
    }
}