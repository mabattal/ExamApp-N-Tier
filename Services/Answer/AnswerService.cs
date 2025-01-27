using ExamApp.Repositories;
using ExamApp.Repositories.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ExamApp.Services.Answer
{
    public class AnswerService(IAnswerRepository answerRepository, IQuestionRepository questionRepository, IUnitOfWork _unitOfWork) : IAnswerService
    {
        public async Task<ServiceResult<CreateAnswerResponseDto>> AddAsync(CreateAnswerRequestDto createAnswerRequest)
        {
            var question = await questionRepository.GetByIdAsync(createAnswerRequest.QuestionId);
            if (question is null)
            {
                return ServiceResult<CreateAnswerResponseDto>.Fail("Question not found", HttpStatusCode.NotFound);
            }

            // `Cevap doğru ise true, yanlış ise false olacak şekilde kontrol edilir`
            var isCorrect = !string.IsNullOrEmpty(createAnswerRequest.SelectedAnswer) &&
                            createAnswerRequest.SelectedAnswer.Equals(question.CorrectAnswer, StringComparison.OrdinalIgnoreCase);

            var answer = new Repositories.Entities.Answer()
            {
                UserId = createAnswerRequest.UserId,
                QuestionId = createAnswerRequest.QuestionId,
                SelectedAnswer = createAnswerRequest.SelectedAnswer,
                IsCorrect = isCorrect
            };
            await answerRepository.AddAsync(answer);
            await _unitOfWork.SaveChangeAsync();

            return ServiceResult<CreateAnswerResponseDto>.Success(new CreateAnswerResponseDto(answer.AnswerId));
        }

        public async Task<ServiceResult> UpdateAsync(int id, UpdateAnswerRequestDto request)
        {
            //Fast fail(önce olumsuz durumları kontrol edip geri dönüş yap)
            var answer = await answerRepository.GetByIdAsync(id);
            if (answer is null)
            {
                return ServiceResult.Fail("Answer not found", HttpStatusCode.NotFound);
            }

            var question = await questionRepository.GetByIdAsync(request.QuestionId);
            if (question is null)
            {
                return ServiceResult.Fail("Question not found", HttpStatusCode.NotFound);
            }

            //Cevap doğru ise true, yanlış ise false olacak şekilde kontrol edilir
            var isCorrect = !string.IsNullOrEmpty(request.SelectedAnswer) &&
                            request.SelectedAnswer.Equals(question.CorrectAnswer, StringComparison.OrdinalIgnoreCase);

            answer.SelectedAnswer = request.SelectedAnswer;
            answer.IsCorrect = request.IsCorrect;

            answerRepository.Update(answer);
            await _unitOfWork.SaveChangeAsync();

            return ServiceResult.Success();
        }

        public async Task<ServiceResult<AnswerResponseDto?>> GetByIdAsync(int id)
        {
            var answer = await answerRepository.GetByIdAsync(id);

            if (answer is null)
            {
                return ServiceResult<AnswerResponseDto>.Fail("Answer not found", HttpStatusCode.NotFound)!;
            }
            var answerAsDto = new AnswerResponseDto(answer.AnswerId, answer.UserId, answer.QuestionId, answer.SelectedAnswer, answer.IsCorrect);

            return ServiceResult<AnswerResponseDto>.Success(answerAsDto)!;
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var answer = await answerRepository.GetByIdAsync(id);
            if (answer is null)
            {
                return ServiceResult.Fail("Answer not found", HttpStatusCode.NotFound);
            }
            answerRepository.Delete(answer);
            await _unitOfWork.SaveChangeAsync();
            return ServiceResult.Success();
        }

        public async Task<ServiceResult<List<AnswerResponseDto>>> GetByUserAndExamAsync(int userId, int examId)
        {
            var answers = await answerRepository.GetByUserAndExam(userId, examId).ToListAsync();
            var answerAsDto = answers.Select(a => new AnswerResponseDto(a.AnswerId, a.UserId, a.QuestionId, a.SelectedAnswer, a.IsCorrect)).ToList();
            return ServiceResult<List<AnswerResponseDto>>.Success(answerAsDto);
        }

    }
}
