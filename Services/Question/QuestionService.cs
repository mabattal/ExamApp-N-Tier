using ExamApp.Repositories.Repositories;
using System.Net;
using ExamApp.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ExamApp.Services.Question
{
    public class QuestionService(IQuestionRepository questionRepository, IUnitOfWork unitOfWork) : IQuestionService
    {
        public async Task<ServiceResult<QuestionResponseDto>> GetByIdAsync(int id)
        {
            var question = await questionRepository.GetByIdAsync(id);
            if (question is null)
            {
                return ServiceResult<QuestionResponseDto>.Fail("Question not found", HttpStatusCode.NotFound);
            }

            var questionAsDto = new QuestionResponseDto(question.QuestionId, question.QuestionText, question.OptionA, question.OptionB, question.OptionC, question.OptionD, question.CorrectAnswer);

            return ServiceResult<QuestionResponseDto>.Success(questionAsDto);
        }

        public async Task<ServiceResult<CreateQuestionResponseDto>> AddAsync(CreateQuestionRequestDto createQuestionRequest)
        {
            var question = new Repositories.Entities.Question()
            {
                ExamId = createQuestionRequest.ExamId,
                QuestionText = createQuestionRequest.QuestionText,
                OptionA = createQuestionRequest.OptionA,
                OptionB = createQuestionRequest.OptionB,
                OptionC = createQuestionRequest.OptionC,
                OptionD = createQuestionRequest.OptionD,
                CorrectAnswer = createQuestionRequest.CorrectAnswer
            };

            await questionRepository.AddAsync(question);
            await unitOfWork.SaveChangeAsync();

            return ServiceResult<CreateQuestionResponseDto>.Success(new CreateQuestionResponseDto(question.QuestionId));
        }

        public async Task<ServiceResult> UpdateAsync(int id, UpdateQuestionRequestDto updateQuestionRequest)
        {
            var question = await questionRepository.GetByIdAsync(id);
            if (question is null)
            {
                return ServiceResult.Fail("Question not found", HttpStatusCode.NotFound);
            }
            question.QuestionText = updateQuestionRequest.QuestionText;
            question.OptionA = updateQuestionRequest.OptionA;
            question.OptionB = updateQuestionRequest.OptionB;
            question.OptionC = updateQuestionRequest.OptionC;
            question.OptionD = updateQuestionRequest.OptionD;
            question.CorrectAnswer = updateQuestionRequest.CorrectAnswer;

            questionRepository.Update(question);
            await unitOfWork.SaveChangeAsync();
            return ServiceResult.Success();
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var question = await questionRepository.GetByIdAsync(id);
            if (question is null)
            {
                return ServiceResult.Fail("Question not found", HttpStatusCode.NotFound);
            }
            questionRepository.Delete(question);
            await unitOfWork.SaveChangeAsync();
            return ServiceResult.Success();
        }

        public async Task<ServiceResult<List<QuestionResponseDto>>> GetByExamIdAsync(int examId)
        {
            var questions = await questionRepository.GetByExamId(examId).ToListAsync();
            var questionsAsDto = questions.Select(q => new QuestionResponseDto(q.QuestionId, q.QuestionText, q.OptionA, q.OptionB, q.OptionC, q.OptionD, q.CorrectAnswer)).ToList();
            return ServiceResult<List<QuestionResponseDto>>.Success(questionsAsDto);
        }
    }
}
