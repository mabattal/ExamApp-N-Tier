using ExamApp.Services.User;

namespace ExamApp.Services.Exam
{
    public record ExamWithInstructorResponseDto(
        int ExamId,
        string Title,
        string Description,
        DateTimeOffset StartDate,
        DateTimeOffset EndDate,
        int Duration,
        UserResponseDto Instructor
    );
}

