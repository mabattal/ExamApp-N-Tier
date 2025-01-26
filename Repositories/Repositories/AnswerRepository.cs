using ExamApp.Repositories.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExamApp.Repositories.Repositories
{
    public class AnswerRepository(AppDbContext context) : GenericRepository<Answer>(context), IAnswerRepository
    {
        public IQueryable<Answer> GetByUserAndExam(int userId, int examId)
        {
            return context.Answers.Where(a => a.UserId == userId && a.Question.ExamId == examId);
        }
    }
}
