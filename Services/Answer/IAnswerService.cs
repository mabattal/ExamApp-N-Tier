namespace ExamApp.Services.Answer
{
    public interface IAnswerService
    {
        //Task<ServiceResult<List<AnswerResponseDto>>> GetAll();
        Task<ServiceResult<AnswerResponseDto>> GetByIdAsync(int id);
        Task<ServiceResult<CreateAnswerResponseDto>> AddAsync(CreateAnswerRequestDto createAnswerRequest);
        Task<ServiceResult> UpdateAsync(int id, UpdateAnswerRequestDto updateAnswerRequest);
        Task<ServiceResult> DeleteAsync(int id);
        Task<ServiceResult<List<AnswerResponseDto>>> GetByUserAndExamAsync(int userId, int examId);     //bir kullanıcının bir sınavdaki tüm cevaplarını getir
        Task<ServiceResult<AnswerStatisticsResponse>> GetAnswerStatisticsAsync(int examId, int userId);
    }
}
