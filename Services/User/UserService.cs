using System.Net;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using ExamApp.Repositories;
using ExamApp.Repositories.Enums;
using ExamApp.Repositories.Users;
using ExamApp.Services.User.Create;
using ExamApp.Services.User.Update;
using Microsoft.EntityFrameworkCore;

namespace ExamApp.Services.User
{
    public class UserService(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper) : IUserService
    {
        public async Task<ServiceResult<List<UserResponseDto>>> GetAllAsync()
        {
            var users = await userRepository.GetAll().Where(u => u.IsDeleted != true).ToListAsync();
            var userAsDto = mapper.Map<List<UserResponseDto>>(users);

            //manuel mapping
            //var userAsDto = users.Select(u => new UserResponseDto(u.UserId, u.FullName, u.Email, u.Role, u.IsDeleted)).ToList();

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
            var userAsDto = mapper.Map<List<UserResponseDto>>(users);
            return ServiceResult<List<UserResponseDto>>.Success(userAsDto);
        }

        public async Task<ServiceResult<UserResponseDto?>> GetByIdOrEmailAsync(string value)
        {
            Repositories.Users.User? user = null;

            if (int.TryParse(value, out int id))
            {
                user = await userRepository.Where(u => u.UserId == id).FirstOrDefaultAsync();
                if (user is null)
                {
                    return ServiceResult<UserResponseDto?>.Fail("User not found.", HttpStatusCode.NotFound);
                }
            }
            else
            {
                user = await userRepository.Where(u => u.Email == value).FirstOrDefaultAsync();
                if (user is null)
                {
                    return ServiceResult<UserResponseDto?>.Fail("User not found.", HttpStatusCode.NotFound);
                }
            }

            var userDto = mapper.Map<UserResponseDto>(user);
            return ServiceResult<UserResponseDto?>.Success(userDto);
        }

        public async Task<ServiceResult<UserResponseDto?>> GetInstructorByIdAsync(int id)
        {
            var instructor = await userRepository.Where(u => u.UserId == id && u.Role == UserRole.Instructor).SingleOrDefaultAsync();
            if (instructor is null)
            {
                return ServiceResult<UserResponseDto>.Fail("Instructor not found or not authorized.", HttpStatusCode.NotFound)!;
            }
            var userAsDto = mapper.Map<UserResponseDto>(instructor);
            return ServiceResult<UserResponseDto>.Success(userAsDto)!;
        }

        public async Task<ServiceResult<CreateUserResponseDto>> AddAsync(CreateUserRequestDto createUserRequest)
        {
            var existingUser = await userRepository.Where(u => u.Email == createUserRequest.Email).AnyAsync();
            if (existingUser) {
                return ServiceResult<CreateUserResponseDto>.Fail("E-mail address already exists", HttpStatusCode.BadRequest);
            }

            var user = mapper.Map<Repositories.Users.User>(createUserRequest);
            user.IsDeleted = false;

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

            user = mapper.Map(updateUserRequest, user);

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
            var userAsDto = mapper.Map<List<UserResponseDto>>(users);
            return ServiceResult<List<UserResponseDto>>.Success(userAsDto);
        }

        public async Task<UserResponseDto?> ValidateUserAsync(string email, string password)
        {
            var user = await userRepository.Where(u => u.Email == email && !u.IsDeleted).SingleOrDefaultAsync();
            if (user is null || !VerifyPassword(password, user.Password))
            {
                return null; // Kullanıcı bulunamazsa veya şifre yanlışsa null döndür
            }

            return new UserResponseDto(user.UserId, user.FullName, user.Email, user.Role, user.IsDeleted);
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            using var sha256 = SHA256.Create();
            var hashedPassword = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
            return hashedPassword == storedHash;
        }
    }
}
