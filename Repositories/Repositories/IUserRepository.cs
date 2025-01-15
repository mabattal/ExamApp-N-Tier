using ExamApp.Repositories.Entities;
using ExamApp.Repositories.Enums;

namespace ExamApp.Repositories.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        IQueryable<User> GetByRole(UserRole role);
    }
}
