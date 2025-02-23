using ExamApp.Repositories.Enums;

namespace ExamApp.Services.User
{
    public record CreateUserRequestDto(
        string FullName,
        string Email,
        UserRole Role,
        string Password);
}
