using ExamApp.Repositories.Entities;
using ExamApp.Repositories.Enums;
using Microsoft.EntityFrameworkCore;

namespace ExamApp.Repositories.Repositories
{
    public class UserRepository(AppDbContext context) : GenericRepository<User>(context), IUserRepository

    {
        public Task<User?> GetByEmailAsync(string email)
        {
            return context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public IQueryable<User> GetByRole(UserRole role)
        {
            return context.Users.Where(x => x.Role == role);
        }
    }
}
