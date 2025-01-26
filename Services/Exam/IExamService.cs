namespace ExamApp.Services.Exam
{
    public interface IExamService
    {
        Task<ServiceResult<CreateExamResponseDto>> AddAsync(CreateExamRequestDto examRequest);
        Task<ServiceResult> UpdateAsync(int id, UpdateExamRequestDto examRequest);
        Task<ServiceResult> DeleteAsync(int id);
        Task<ServiceResult<List<ExamResponseDto>>> GetByInstructorAsync(int instructorId);          // Eğitmene ait sınavları listelemek için
        Task<ServiceResult<List<ExamResponseDto>>> GetActiveExamsAsync();                           // Şu an aktif olan sınavları getirmek için
        Task<ServiceResult<ExamWithDetailsResponseDto?>> GetExamWithDetailsAsync(int examId);       // Sorular ve eğitmen bilgisi ile sınav detaylarını getirmek için
    }
}
