using ExamApp.Repositories.Entities;

namespace ExamApp.Repositories.Repositories
{
    public class UserRepository(AppDbContext context) : GenericRepository<User>(context), IUserRepository
    {
        
    }
}
