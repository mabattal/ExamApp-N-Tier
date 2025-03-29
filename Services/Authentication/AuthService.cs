using ExamApp.Repositories;
using ExamApp.Repositories.Enums;
using ExamApp.Repositories.Users;
using Microsoft.EntityFrameworkCore;
using System.Net;
using ExamApp.Services.Helpers;

namespace ExamApp.Services.Authentication
{
    public class AuthService(IUserRepository userRepository, JwtService jwtService, IUnitOfWork unitOfWork) : IAuthService
    {
        public async Task<ServiceResult<UserTokenResponseDto>> ValidateUserAsync(string email, string password)
        {
            var user = await userRepository.Where(u => u.Email == email && !u.IsDeleted).SingleOrDefaultAsync();
            if (user is null)
            {
                return ServiceResult<UserTokenResponseDto>.Fail("Email not found.", HttpStatusCode.Unauthorized);
            }
            if (!PasswordHasher.Verify(password, user.Password))
            {
                return ServiceResult<UserTokenResponseDto>.Fail("Invalid password.", HttpStatusCode.Unauthorized);
            }

            var token = jwtService.GenerateToken(user.UserId, user.Role.ToString());

            return ServiceResult<UserTokenResponseDto>.Success(new UserTokenResponseDto(token, user.Role.ToString(), user.FullName!));
        }

        public async Task<ServiceResult> RegisterAsync(RegisterUserRequestDto request)
        {
            var existingUser = await userRepository.Where(u => u.Email == request.Email).FirstOrDefaultAsync();
            if (existingUser != null)
            {
                return ServiceResult.Fail("This email is already in use.", HttpStatusCode.BadRequest);
            }

            var hashedPassword = PasswordHasher.Hash(request.Password);

            var newUser = new Repositories.Users.User
            {
                Email = request.Email,
                Password = hashedPassword,
                Role = UserRole.Student,
                IsDeleted = false
            };

            await userRepository.AddAsync(newUser);
            await unitOfWork.SaveChangeAsync();

            return ServiceResult.Success(HttpStatusCode.Created);
        }
    }
}
