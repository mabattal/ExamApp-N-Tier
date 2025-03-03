using ExamApp.Repositories.Enums;

namespace ExamApp.Services.User.Update
{
    public record UpdateUserRequestDto(
        string FullName,
        string Email,
        UserRole Role,
        string Password
        );
}
