using ExamApp.Services.Question;
using ExamApp.Services.User;

namespace ExamApp.Services.Exam
{
    public record ExamWithDetailsResponseDto(
        int ExamId,
        string Title,
        string Description,
        DateTimeOffset StartDate,
        DateTimeOffset EndDate,
        int Duration,
        UserResponseDto Instructor,
        ICollection<QuestionResponseDto> Questions
    );
}