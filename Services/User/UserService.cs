using System.Net;
using ExamApp.Repositories;
using ExamApp.Repositories.Enums;
using ExamApp.Repositories.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ExamApp.Services.User
{
    public class UserService(IUserRepository userRepository, IUnitOfWork unitOfWork) : IUserService
    {
        public async Task<ServiceResult<List<UserResponseDto>>> GetAllAsync()
        {
            var users = await userRepository.GetAll().Where(u => u.IsDeleted != true).ToListAsync();
            var userAsDto = users.Select(u => new UserResponseDto(u.UserId, u.FullName, u.Email, u.Role, u.IsDeleted)).ToList();

            return ServiceResult<List<UserResponseDto>>.Success(userAsDto);
        }

        public async Task<ServiceResult<UserResponseDto?>> GetByIdAsync(int id)
        {
            var user = await userRepository.Where(u => u.UserId == id && u.IsDeleted != true).FirstOrDefaultAsync();
            if (user is null)
            {
                return ServiceResult<UserResponseDto>.Fail("User not found", HttpStatusCode.NotFound)!;
            }
            var userAsDto = new UserResponseDto(user.UserId, user.FullName, user.Email, user.Role, user.IsDeleted);
            return ServiceResult<UserResponseDto>.Success(userAsDto)!;
        }

        public async Task<ServiceResult<CreateUserResponseDto>> AddAsync(CreateUserRequestDto createUserRequest)
        {
            var user = new Repositories.Entities.User()
            {
                FullName = createUserRequest.FullName,
                Email = createUserRequest.Email,
                PasswordHash = createUserRequest.PasswordHash,
                Role = createUserRequest.Role,
                IsDeleted = false
            };
            await userRepository.AddAsync(user);
            await unitOfWork.SaveChangeAsync();
            return ServiceResult<CreateUserResponseDto>.Success(new CreateUserResponseDto(user.UserId));
        }

        public async Task<ServiceResult> UpdateAsync(int id, UpdateUserRequestDto updateUserRequest)
        {
            var user = await userRepository.GetByIdAsync(id);
            if (user is null)
            {
                return ServiceResult.Fail("User not found", HttpStatusCode.NotFound);
            }
            user.FullName = updateUserRequest.FullName;
            user.Email = updateUserRequest.Email;
            user.PasswordHash = updateUserRequest.PasswordHash;
            user.Role = updateUserRequest.Role;

            userRepository.Update(user);
            await unitOfWork.SaveChangeAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var user = await userRepository.GetByIdAsync(id);
            if (user is null)
            {
                return ServiceResult.Fail("User not found", HttpStatusCode.NotFound);
            }
            user.IsDeleted = true;

            userRepository.Update(user);
            await unitOfWork.SaveChangeAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult<UserResponseDto?>> GetByEmailAsync(string email)
        {
            var user = await userRepository.Where(u => u.Email == email && u.IsDeleted != true).FirstOrDefaultAsync();
            if (user is null)
            {
                return ServiceResult<UserResponseDto>.Fail("User not found", HttpStatusCode.NotFound)!;
            }
            var userAsDto = new UserResponseDto(user.UserId, user.FullName, user.Email, user.Role, user.IsDeleted);
            return ServiceResult<UserResponseDto>.Success(userAsDto)!;
        }

        public async Task<ServiceResult<List<UserResponseDto>>> GetByRole(UserRole role)
        {
            var users = await userRepository.Where(u => u.Role == role && u.IsDeleted != true).ToListAsync();
            var userAsDto = users.Select(u => new UserResponseDto(u.UserId, u.FullName, u.Email, u.Role, u.IsDeleted)).ToList();
            return ServiceResult<List<UserResponseDto>>.Success(userAsDto);
        }
    }
}
