namespace ExamApp.Repositories.ExamResults
{
    public interface IExamResultRepository : IGenericRepository<ExamResult>
    {
        Task<decimal> GetAverageScoreByExamAsync(int examId);        //genel başarı durumu
        Task<decimal> GetMaxScoreByExamAsync(int examId);            //en yüksek puan
        Task<decimal> GetMinScoreByExamAsync(int examId);            //en düşük puan
        Task<int> GetExamCountByExamAsync(int examId);               //sınava giren öğrenci sayısı
        IQueryable<ExamResult> GetByUserId(int userId);
        IQueryable<ExamResult> GetByUserIdAndExamId(int userId, int examId);
    }

}
