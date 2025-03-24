using ExamApp.Repositories.Database;

namespace ExamApp.Repositories.Users
{
    public class UserRepository(AppDbContext context) : GenericRepository<User>(context), IUserRepository
    {
        
    }
}
