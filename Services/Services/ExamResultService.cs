using ExamApp.Repositories.Repositories;

namespace ExamApp.Services.Services
{
    public class ExamResultService(IExamResultRepository examResultRepository) : IExamResultService
    {
    }
}
