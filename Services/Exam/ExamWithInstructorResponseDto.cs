using ExamApp.Services.User;

namespace ExamApp.Services.Exam
{
    public record ExamWithInstructorResponseDto(
        int ExamId,
        string Title,
        string Description,
        DateTime StartDate,
        DateTime EndDate,
        int Duration,
        UserResponseDto Instructors
    );
}

