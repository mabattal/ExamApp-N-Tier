using ExamApp.Services.Question.Create;
using ExamApp.Services.Question.Update;

namespace ExamApp.Services.Question
{
    public interface IQuestionService
    {
        Task<ServiceResult<QuestionResponseDto?>> GetByIdAsync(int id);
        Task<ServiceResult<CreateQuestionResponseDto>> AddAsync(CreateQuestionRequestDto createQuestionRequest, int userId);
        Task<ServiceResult> UpdateAsync(int id, UpdateQuestionRequestDto updateQuestionRequest, int userId);
        Task<ServiceResult> DeleteAsync(int id, int userId);
        Task<ServiceResult<List<QuestionResponseWithoutCorrectAnswerDto>>> GetByExamIdAsync(int examId);
        Task<ServiceResult<List<QuestionResponseDto>>> GetByExamIdWithCorrectAnswerAsync(int examId);

    }
}
