using ExamApp.Repositories.Repositories;
using System.Net;
using AutoMapper;
using ExamApp.Repositories;
using ExamApp.Services.Exam;
using Microsoft.EntityFrameworkCore;
using ExamApp.Services.Question.Create;
using ExamApp.Services.Question.Update;

namespace ExamApp.Services.Question
{
    public class QuestionService(
        IQuestionRepository questionRepository, 
        IExamService examService, 
        IUnitOfWork unitOfWork,
        IMapper mapper) : IQuestionService
    {
        public async Task<ServiceResult<QuestionResponseDto?>> GetByIdAsync(int id)
        {
            var question = await questionRepository.GetByIdAsync(id);
            if (question is null)
            {
                return ServiceResult<QuestionResponseDto>.Fail("Question not found", HttpStatusCode.NotFound)!;
            }

            var exam = await examService.GetByIdAsync(question.ExamId);
            if (exam.IsFail)
            {
                return ServiceResult<QuestionResponseDto>.Fail("The exam of the question has been deleted", HttpStatusCode.BadRequest)!;
            }

            var questionAsDto = mapper.Map<QuestionResponseDto>(question);

            return ServiceResult<QuestionResponseDto>.Success(questionAsDto)!;
        }

        public async Task<ServiceResult<CreateQuestionResponseDto>> AddAsync(CreateQuestionRequestDto createQuestionRequest)
        {
            var exam = await examService.GetByIdAsync(createQuestionRequest.ExamId);
            if (exam.IsFail)
            {
                return ServiceResult<CreateQuestionResponseDto>.Fail(exam.ErrorMessage!, exam.Status);
            }

            var questionResult = await questionRepository.Where(x => x.ExamId == createQuestionRequest.ExamId && x.QuestionText == createQuestionRequest.QuestionText).AnyAsync();
            if (questionResult)
            {
                return ServiceResult<CreateQuestionResponseDto>.Fail("Question already exists in this exam.", HttpStatusCode.BadRequest)!;
            }

            var question = mapper.Map<Repositories.Entities.Question>(createQuestionRequest);

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

            //question.QuestionText = updateQuestionRequest.QuestionText;
            //question.OptionA = updateQuestionRequest.OptionA;
            //question.OptionB = updateQuestionRequest.OptionB;
            //question.OptionC = updateQuestionRequest.OptionC;
            //question.OptionD = updateQuestionRequest.OptionD;
            //question.CorrectAnswer = updateQuestionRequest.CorrectAnswer;

            question = mapper.Map(updateQuestionRequest, question);

            questionRepository.Update(question);
            await unitOfWork.SaveChangeAsync();
            return ServiceResult.Success(HttpStatusCode.NoContent);
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
            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult<List<QuestionResponseWithoutCorrectAnswerDto>>> GetByExamIdAsync(int examId)
        {
            var exam = await examService.GetByIdAsync(examId);
            if (exam.IsFail)
            {
                return ServiceResult<List<QuestionResponseWithoutCorrectAnswerDto>>.Fail(exam.ErrorMessage!, exam.Status);
            }

            var questions = await questionRepository.GetByExamId(examId).ToListAsync();
            if (!questions.Any())
            {
                return ServiceResult<List<QuestionResponseWithoutCorrectAnswerDto>>.Fail("No questions found for the given exam.", HttpStatusCode.NotFound);
            }

            var questionsAsDto = mapper.Map<List<QuestionResponseWithoutCorrectAnswerDto>>(questions);
            return ServiceResult<List<QuestionResponseWithoutCorrectAnswerDto>>.Success(questionsAsDto);
        }
    }
}
