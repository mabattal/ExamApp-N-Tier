using ExamApp.Repositories.Repositories;

namespace ExamApp.Services.ExamResult
{
    public class ExamResultService(IExamResultRepository examResultRepository) : IExamResultService
    {
    }
}
