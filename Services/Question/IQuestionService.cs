namespace ExamApp.Services.Question
{
    public interface IQuestionService
    {
        Task<ServiceResult<QuestionResponseDto?>> GetByIdAsync(int id);
        Task<ServiceResult<CreateQuestionResponseDto>> AddAsync(CreateQuestionRequestDto createQuestionRequest);
        Task<ServiceResult> UpdateAsync(int id, UpdateQuestionRequestDto updateQuestionRequest);
        Task<ServiceResult> DeleteAsync(int id);
        Task<ServiceResult<List<QuestionResponseWithoutCorrectAnswerDto>>> GetByExamIdAsync(int examId);

    }
}
