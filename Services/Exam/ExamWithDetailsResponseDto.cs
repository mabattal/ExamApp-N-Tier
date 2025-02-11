using ExamApp.Services.Question;
using ExamApp.Services.User;

namespace ExamApp.Services.Exam
{
    public record ExamWithDetailsResponseDto(
        int ExamId,
        string Title,
        string Description,
        DateTime StartDate,
        DateTime EndDate,
        int Duration,
        UserResponseDto Instructors,
        ICollection<QuestionResponseDto> Questions
    );
}