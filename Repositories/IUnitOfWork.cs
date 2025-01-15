namespace ExamApp.Repositories
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangeAsync();
    }
}
