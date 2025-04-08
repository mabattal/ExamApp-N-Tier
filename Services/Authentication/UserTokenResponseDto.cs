namespace ExamApp.Services.Authentication
{
    public record UserTokenResponseDto(
        string Token,
        string Role,
        string FullName,
        string Email,
        int UserId);
}
