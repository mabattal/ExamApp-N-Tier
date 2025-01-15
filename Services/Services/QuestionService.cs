using ExamApp.Repositories.Repositories;

namespace ExamApp.Services.Services
{
    public class QuestionService(IQuestionRepository questionRepository) : IQuestionService
    {
    }
}
