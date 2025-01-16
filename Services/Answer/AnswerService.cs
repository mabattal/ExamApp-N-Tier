using ExamApp.Repositories;
using ExamApp.Repositories.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ExamApp.Services.Answer
{
    public class AnswerService(IAnswerRepository _answerRepository, IUnitOfWork _unitOfWork) : IAnswerService
    {
        public async Task<ServiceResult<CreateAnswerResponseDto>> AddAsync(CreateAnswerRequestDto createAnswerRequest)
        {
            var answer = new Repositories.Entities.Answer()
            {
                UserId = createAnswerRequest.UserId,
                QuestionId = createAnswerRequest.QuestionId,
                SelectedAnswer = createAnswerRequest.SelectedAnswer,
                IsCorrect = createAnswerRequest.IsCorrect
            };
            await _answerRepository.AddAsync(answer);
            await _unitOfWork.SaveChangeAsync();

            return ServiceResult<CreateAnswerResponseDto>.Success(new CreateAnswerResponseDto(answer.AnswerId));
        }

        public async Task<ServiceResult> UpdateAsync(int id, UpdateAnswerRequestDto request)
        {
            var answer = await _answerRepository.GetByIdAsync(id);

            //Fast fail(önce olumsuz durumları kontrol edip geri dönüş yap)
            if (answer is null)
            {
                return ServiceResult.Fail("Answer not found", HttpStatusCode.NotFound);
            }

            answer.SelectedAnswer = request.SelectedAnswer;
            answer.IsCorrect = request.IsCorrect;

            _answerRepository.Update(answer);
            await _unitOfWork.SaveChangeAsync();

            return ServiceResult.Success();
        }

        public async Task<ServiceResult<List<AnswerResponseDto>>> GetAll()
        {
            var answers = await _answerRepository.GetAll().ToListAsync();
            var answerAsDto = answers.Select(a => new AnswerResponseDto(a.AnswerId, a.UserId, a.QuestionId, a.SelectedAnswer, a.IsCorrect)).ToList();

            return ServiceResult<List<AnswerResponseDto>>.Success(answerAsDto);
        }

        public async Task<ServiceResult<AnswerResponseDto>> GetByIdAsync(int id)
        {
            var answer = await _answerRepository.GetByIdAsync(id);

            if (answer is null)
            {
                return ServiceResult<AnswerResponseDto>.Fail("Answer not found", HttpStatusCode.NotFound);
            }
            var answerAsDto = new AnswerResponseDto(answer.AnswerId, answer.UserId, answer.QuestionId, answer.SelectedAnswer, answer.IsCorrect);

            return ServiceResult<AnswerResponseDto>.Success(answerAsDto);
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var answer = await _answerRepository.GetByIdAsync(id);
            if (answer is null)
            {
                return ServiceResult.Fail("Answer not found", HttpStatusCode.NotFound);
            }
            _answerRepository.Delete(answer);
            await _unitOfWork.SaveChangeAsync();
            return ServiceResult.Success();
        }

        public async Task<ServiceResult<List<AnswerResponseDto>>> GetByUserAndExamAsync(int userId, int examId)
        {
            var answers = await _answerRepository.GetByUserAndExam(userId, examId).ToListAsync();
            var answerAsDto = answers.Select(a => new AnswerResponseDto(a.AnswerId, a.UserId, a.QuestionId, a.SelectedAnswer, a.IsCorrect)).ToList();
            return ServiceResult<List<AnswerResponseDto>>.Success(answerAsDto);
        }

        public async Task<ServiceResult<AnswerStatisticsResponse>> GetAnswerStatisticsAsync(int examId, int userId)
        {
            var (correct, incorrect) = await _answerRepository.GetAnswerStatisticsAsync(examId, userId);
            var answerStatistics = new AnswerStatisticsResponse(correct, incorrect);

            return ServiceResult<AnswerStatisticsResponse>.Success(answerStatistics);
        }


    }
}
