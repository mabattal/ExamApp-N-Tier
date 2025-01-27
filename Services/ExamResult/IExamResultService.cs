namespace ExamApp.Services.ExamResult
{
    public interface IExamResultService
    {
        Task<ServiceResult<ExamResultResponseDto?>> GetByIdAsync(int id);
        Task<ServiceResult> StartExamAsync(int examId, int userId);
        Task<ServiceResult> SubmitExamAsync(int examId, int userId);
        Task<ServiceResult<List<ExamResultResponseDto>>> GetByUserIdAsync(int userId);
        Task<ServiceResult<ExamResultAverageScoreResponseDto>> GetAverageScoreByExamAsync(int examId);
    }
}
