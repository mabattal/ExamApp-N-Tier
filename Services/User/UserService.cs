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

        public async Task<ServiceResult<List<UserResponseDto>>> GetPagedAllAsync(int pageNumber, int pageSize)
        {
            // pageNumber - pageSize
            // 1 - 10 => 0, 10  kayıt    skip(0).take(10)
            // 2 - 10 => 11, 20 kayıt    skip(10).take(10)
            // 3 - 10 => 21, 30 kayıt    skip(20).take(10)
            // 4 - 10 => 31, 40 kayıt    skip(30).take(10)

            var users = await userRepository.GetAll().Where(u => u.IsDeleted != true).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            var userAsDto = users.Select(u => new UserResponseDto(u.UserId, u.FullName, u.Email, u.Role, u.IsDeleted)).ToList();
            return ServiceResult<List<UserResponseDto>>.Success(userAsDto);
        }

        public async Task<ServiceResult<UserResponseDto?>> GetByIdOrEmailAsync(int? id, string? email)
        {
            Repositories.Entities.User? user = null;

            if (id is not null)
            {
                user = await userRepository.Where(u => u.UserId == id && u.IsDeleted != true).FirstOrDefaultAsync();
            }
            else if (!string.IsNullOrEmpty(email))
            {
                user = await userRepository.Where(u => u.Email == email && u.IsDeleted != true).FirstOrDefaultAsync();
            }

            if (user is null)
            {
                return ServiceResult<UserResponseDto?>.Fail("User not found.", HttpStatusCode.NotFound);
            }

            var userDto = new UserResponseDto(user.UserId, user.FullName, user.Email, user.Role, user.IsDeleted);
            return ServiceResult<UserResponseDto?>.Success(userDto);
        }

        public async Task<ServiceResult<UserResponseDto?>> GetInstructorByIdAsync(int id)
        {
            var instructor = await userRepository.Where(u => u.UserId == id && u.IsDeleted != true && u.Role == UserRole.Instructor).SingleOrDefaultAsync();
            if (instructor is null)
            {
                return ServiceResult<UserResponseDto>.Fail("Instructor not found or not authorized.", HttpStatusCode.NotFound)!;
            }
            var userAsDto = new UserResponseDto(instructor.UserId, instructor.FullName, instructor.Email, instructor.Role, instructor.IsDeleted);
            return ServiceResult<UserResponseDto>.Success(userAsDto)!;
        }

        public async Task<ServiceResult<CreateUserResponseDto>> AddAsync(CreateUserRequestDto createUserRequest)
        {
            var existingUser = await userRepository.Where(u => u.Email == createUserRequest.Email).AnyAsync();
            if (existingUser) {
                return ServiceResult<CreateUserResponseDto>.Fail("E-mail address already exists", HttpStatusCode.BadRequest);
            }

            var user = new Repositories.Entities.User()
            {
                FullName = createUserRequest.FullName,
                Email = createUserRequest.Email,
                Password = createUserRequest.Password,
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

            var existingUser = await userRepository.Where(u => u.Email == updateUserRequest.Email && u.UserId != id).AnyAsync();
            if (existingUser)
            {
                return ServiceResult.Fail("E-mail address already exists", HttpStatusCode.BadRequest);
            }

            user.FullName = updateUserRequest.FullName;
            user.Email = updateUserRequest.Email;
            user.Password = updateUserRequest.Password;
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

        public async Task<ServiceResult<List<UserResponseDto>>> GetByRole(UserRole role)
        {
            var users = await userRepository.Where(u => u.Role == role && u.IsDeleted != true).ToListAsync();
            var userAsDto = users.Select(u => new UserResponseDto(u.UserId, u.FullName, u.Email, u.Role, u.IsDeleted)).ToList();
            return ServiceResult<List<UserResponseDto>>.Success(userAsDto);
        }

        
    }
}
