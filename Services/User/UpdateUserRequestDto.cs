using ExamApp.Repositories.Enums;

namespace ExamApp.Services.User
{
    public record UpdateUserRequestDto(
        string FullName,
        string Email,
        UserRole Role,
        string PasswordHash
        );
}
