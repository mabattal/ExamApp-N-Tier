using ExamApp.Repositories.Repositories;

namespace ExamApp.Services.Services
{
    public class ExamService(IExamRepository examRepository) : IExamService
    {
    }
}
