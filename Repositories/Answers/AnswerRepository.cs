using Microsoft.EntityFrameworkCore;

namespace ExamApp.Repositories.Answers
{
    public class AnswerRepository(AppDbContext context) : GenericRepository<Answer>(context), IAnswerRepository
    {
        public IQueryable<Answer> GetByUserAndExam(int userId, int examId)
        {
            return context.Answers.Where(a => a.UserId == userId && a.Question.ExamId == examId);
        }

        public IQueryable<Answer?> GetByUserAndQuestion(int userId, int questionId)
        {
            return context.Answers.Where(a => a.UserId == userId && a.QuestionId == questionId);
        }

        public async Task<Answer?> GetByIdWithDetailsAsync(int id)
        {
            return await context.Answers
                .Include(a => a.User)
                .Include(a => a.Question)
                .ThenInclude(q => q.Exam)
                .FirstOrDefaultAsync(a => a.AnswerId == id);
        }
    }
}
