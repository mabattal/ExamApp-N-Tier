using ExamApp.Repositories.Repositories;

namespace ExamApp.Services.User
{
    public class UserService(IUserRepository userRepository) : IUserService
    {
    }
}
