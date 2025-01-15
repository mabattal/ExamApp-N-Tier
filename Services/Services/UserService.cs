using ExamApp.Repositories.Repositories;

namespace ExamApp.Services.Services
{
    public class UserService(IUserRepository userRepository) : IUserService
    {
    }
}
