using ExamApp.Repositories.Enums;

namespace ExamApp.Services.User
{
    public record UserResponseDto(
        int UserId,
        string FullName,
        string Email,
        UserRole Role,
        bool IsDeleted
    );
}