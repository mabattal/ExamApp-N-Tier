using ExamApp.Repositories.Repositories;

namespace ExamApp.Services.Exam
{
    public class ExamService(IExamRepository examRepository) : IExamService
    {
    }
}
