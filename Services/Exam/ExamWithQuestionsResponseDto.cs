using ExamApp.Services.Question;

namespace ExamApp.Services.Exam
{
    public record ExamWithQuestionsResponseDto(
        int ExamId,
        string Title,
        string Description,
        DateTime StartDate,
        DateTime EndDate,
        int Duration,
        ICollection<QuestionResponseDto> Questions
    );
}
