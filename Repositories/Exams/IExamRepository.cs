namespace ExamApp.Repositories.Exams
{
    public interface IExamRepository : IGenericRepository<Exam>
    {
        IQueryable<Exam> GetByInstructor(int instructorId);
        IQueryable<Exam> GetActiveExams();                  //StartDate <= Now && EndDate >= Now
        Task<Exam?> GetExamWithDetailsAsync(int examId);    //Sınava ait detayları (sorular ve eğitmen bilgisi ile birlikte) getirme.
    }
}
