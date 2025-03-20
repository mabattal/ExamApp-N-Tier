namespace ExamApp.Services.Authentication
{
    public record RegisterUserRequestDto(
        string Email,
        string Password);
}
