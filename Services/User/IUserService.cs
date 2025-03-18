using ExamApp.Repositories.Enums;
using ExamApp.Services.User.Create;
using ExamApp.Services.User.Update;

namespace ExamApp.Services.User
{
    public interface IUserService
    {
        Task<ServiceResult<List<UserResponseDto>>> GetAllAsync();
        Task<ServiceResult<List<UserResponseDto>>> GetPagedAllAsync(int pageNumber, int pageSize);
        Task<ServiceResult<UserResponseDto?>> GetByIdOrEmailAsync(string value);
        Task<ServiceResult<UserResponseDto?>> GetInstructorByIdAsync(int id);
        Task<ServiceResult<CreateUserResponseDto>> AddAsync(CreateUserRequestDto createUserRequest);
        Task<ServiceResult> UpdateAsync(int id, UpdateUserRequestDto updateUserRequest);
        Task<ServiceResult> DeleteAsync(int id);
        Task<ServiceResult<List<UserResponseDto>>> GetByRole(UserRole role);
        Task<UserResponseDto?> ValidateUserAsync(string email, string password);

    }
}
