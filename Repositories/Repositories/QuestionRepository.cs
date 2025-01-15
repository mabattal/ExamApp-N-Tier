using ExamApp.Repositories.Entities;

namespace ExamApp.Repositories.Repositories
{
    public class QuestionRepository(AppDbContext context) : GenericRepository<Question>(context), IQuestionRepository
    {
        public IQueryable<Question> GetByExamId(int examId)
        {
            return context.Questions.Where(q => q.ExamId == examId);
        }
    }
}