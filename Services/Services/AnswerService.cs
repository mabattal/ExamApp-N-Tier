using ExamApp.Repositories.Repositories;

namespace ExamApp.Services.Services
{
    public class AnswerService(IAnswerRepository answerRepository) : IAnswerService
    {
    }
}
