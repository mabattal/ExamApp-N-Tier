namespace ExamApp.Services.Exam
{
    public interface IExamService
    {
        Task<ServiceResult<CreateExamResponseDto>> AddAsync(CreateExamRequestDto examRequest);
        Task<ServiceResult> UpdateAsync(int id, UpdateExamRequestDto examRequest);
        Task<ServiceResult> DeleteAsync(int id);
        Task<ServiceResult<List<ExamWithQuestionsResponseDto>>> GetByInstructorAsync(int instructorId);          // Eğitmene ait sınavları listelemek için
        Task<ServiceResult<List<ExamWithInstructorResponseDto>>> GetActiveExamsAsync();                           // Şu an aktif olan sınavları getirmek için
        Task<ServiceResult<ExamWithDetailsResponseDto?>> GetByIdAsync(int id);                                 // Id'ye göre sınav getirmek için
    }
}
