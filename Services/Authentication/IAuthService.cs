using ExamApp.Services.User;

namespace ExamApp.Services.Authentication
{
    public interface IAuthService
    {
        Task<ServiceResult<UserTokenResponseDto>> ValidateUserAsync(string email, string password);
        Task<ServiceResult> RegisterAsync(RegisterUserRequestDto request);
    }

}
