using ExamApp.Repositories.Enums;

namespace ExamApp.Services.User
{
    public record UpdateUserRequestDto(
        int Id,
        string FullName,
        string Email,
        UserRole Role,
        string PasswordHash,
        bool IsDeleted
        );
}
