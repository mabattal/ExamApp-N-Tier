using ExamApp.Services.Question;

namespace ExamApp.Services.Exam
{
    public record ExamWithQuestionsResponseDto(
        int ExamId,
        string Title,
        string Description,
        DateTimeOffset StartDate,
        DateTimeOffset EndDate,
        int Duration,
        ICollection<QuestionResponseDto> Questions
    );
}
