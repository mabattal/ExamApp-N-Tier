using ExamApp.Repositories.Repositories;

namespace ExamApp.Services.Question
{
    public class QuestionService(IQuestionRepository questionRepository) : IQuestionService
    {
    }
}
