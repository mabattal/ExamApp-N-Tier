using ExamApp.Repositories.Enums;

namespace ExamApp.Services.User
{
    public interface IUserService
    {
        Task<ServiceResult<List<UserResponseDto>>> GetAllAsync();
        Task<ServiceResult<UserResponseDto?>> GetByIdAsync(int id);
        Task<ServiceResult<CreateUserResponseDto>> AddAsync(CreateUserRequestDto createUserRequest);
        Task<ServiceResult> UpdateAsync(int id, UpdateUserRequestDto updateUserRequest);
        Task<ServiceResult> DeleteAsync(int id);
        Task<ServiceResult<UserResponseDto?>> GetByEmailAsync(string email);
        Task<ServiceResult<List<UserResponseDto>>> GetByRole(UserRole role);

    }
}
