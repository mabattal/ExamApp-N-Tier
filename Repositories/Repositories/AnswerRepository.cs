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

        public async Task<(int Correct, int Incorrect)> GetAnswerStatisticsAsync(int examId, int userId)
        {
            var answers = await context.Answers.Where(a => a.UserId == userId && a.Question.ExamId == examId).ToListAsync();
            var correct = answers.Count(a => a.IsCorrect);
            var incorrect = answers.Count(a => !a.IsCorrect);

            return (correct, incorrect);
        }
    }
}
