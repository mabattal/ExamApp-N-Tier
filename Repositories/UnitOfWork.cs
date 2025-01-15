namespace ExamApp.Repositories
{
    public class UnitOfWork(AppDbContext context) : IUnitOfWork
    {
        public Task<int> SaveChangeAsync()
        {
            return context.SaveChangesAsync();
        }
    }
}
