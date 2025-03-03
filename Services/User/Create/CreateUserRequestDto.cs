using ExamApp.Repositories.Enums;

namespace ExamApp.Services.User.Create
{
    public record CreateUserRequestDto(
        string FullName,
        string Email,
        UserRole Role,
        string Password);
}
