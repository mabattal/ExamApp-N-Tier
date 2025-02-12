﻿using ExamApp.Repositories;
using ExamApp.Repositories.Repositories;
using ExamApp.Services.Question;
using Microsoft.EntityFrameworkCore;
using System.Net;
using ExamApp.Services.Exam;
using ExamApp.Services.ExamResult;

namespace ExamApp.Services.Answer
{
    public class AnswerService(IAnswerRepository answerRepository, IQuestionService questionService, Lazy<IExamResultService> examResultService, IExamService examService, IUnitOfWork unitOfWork) : IAnswerService
    {
        public async Task<ServiceResult<CreateAnswerResponseDto>> AddAsync(CreateAnswerRequestDto createAnswerRequest)
        {
            var exam = await examService.GetByIdAsync(createAnswerRequest.ExamId);
            if (exam.IsFail)
            {
                return ServiceResult<CreateAnswerResponseDto>.Fail(exam.ErrorMessage, exam.Status);
            }

            var examResult = await examResultService.Value.GetByUserIdAndExamId(createAnswerRequest.UserId, createAnswerRequest.ExamId);
            if (examResult.IsFail)
            {
                return ServiceResult<CreateAnswerResponseDto>.Fail("Exam has not been started for this user.", HttpStatusCode.NotFound);
            }

            var question = await questionService.GetByIdAsync(createAnswerRequest.QuestionId);
            if (question.IsFail)
            {
                return ServiceResult<CreateAnswerResponseDto>.Fail(question.ErrorMessage, question.Status);
            }

            if (question.Data.ExamId != createAnswerRequest.ExamId)
            {
                return ServiceResult<CreateAnswerResponseDto>.Fail("Question does not belong to the exam.", HttpStatusCode.BadRequest);
            }

            var existingAnswer = await answerRepository.GetByUserAndQuestion(createAnswerRequest.UserId, createAnswerRequest.QuestionId).SingleOrDefaultAsync();
            if (existingAnswer != null)
            {
                return ServiceResult<CreateAnswerResponseDto>.Fail("Answer already exists", HttpStatusCode.BadRequest);
            }

            if (exam.Data.StartDate > DateTime.Now)
            {
                return ServiceResult<CreateAnswerResponseDto>.Fail("Exam has not started yet.", HttpStatusCode.BadRequest);
            }

            // Kullanıcı için sınav süresi mi doldu, yoksa sınav zaten bitti mi?
            var examEndTime = examResult.Data.StartDate.AddMinutes(exam.Data.Duration);
            var finalExamEndTime = exam.Data.EndDate < examEndTime ? exam.Data.EndDate : examEndTime;
            if (DateTime.Now > finalExamEndTime)
            {
                return ServiceResult<CreateAnswerResponseDto>.Fail("Exam time is up! You cannot submit answers.", HttpStatusCode.BadRequest);
            }

            // `Cevap doğru ise true, yanlış ise false olacak şekilde kontrol edilir`
            var isCorrect = !string.IsNullOrEmpty(createAnswerRequest.SelectedAnswer) &&
                            createAnswerRequest.SelectedAnswer.Equals(question.Data.CorrectAnswer, StringComparison.OrdinalIgnoreCase);

            var answer = new Repositories.Entities.Answer()
            {
                UserId = createAnswerRequest.UserId,
                QuestionId = createAnswerRequest.QuestionId,
                SelectedAnswer = createAnswerRequest.SelectedAnswer,
                CreatedDate = DateTime.Now,
                IsCorrect = isCorrect
            };
            await answerRepository.AddAsync(answer);
            await unitOfWork.SaveChangeAsync();

            return ServiceResult<CreateAnswerResponseDto>.Success(new CreateAnswerResponseDto(answer.AnswerId));
        }

        public async Task<ServiceResult> UpdateAsync(int id, UpdateAnswerRequestDto request)
        {
            var answer = await answerRepository.GetByIdWithDetailsAsync(id);
            if (answer is null)
            {
                return ServiceResult.Fail("Answer not found", HttpStatusCode.NotFound);
            }

            var exam = await examService.GetByIdAsync(answer.Question!.ExamId);
            if (exam.IsFail)
            {
                return ServiceResult.Fail(exam.ErrorMessage!, exam.Status);
            }

            var examResult = await examResultService.Value.GetByUserIdAndExamId(answer.UserId, answer.Question.ExamId);
            if (examResult.IsFail)
            {
                return ServiceResult.Fail("Exam has not been started for this user.", HttpStatusCode.NotFound);
            }

            var question = await questionService.GetByIdAsync(answer.QuestionId);
            if (question.IsFail)
            {
                return ServiceResult.Fail(question.ErrorMessage!, question.Status);
            }

            if (exam.Data!.StartDate > DateTime.Now)
            {
                return ServiceResult.Fail("Exam has not started yet.", HttpStatusCode.BadRequest);
            }

            var examEndTime = examResult.Data!.StartDate.AddMinutes(exam.Data.Duration);
            var finalExamEndTime = exam.Data.EndDate < examEndTime ? exam.Data.EndDate : examEndTime;
            if (DateTime.Now > finalExamEndTime)
            {
                return ServiceResult.Fail("Exam has already ended", HttpStatusCode.BadRequest);
            }

            //Cevap doğru ise true, yanlış ise false olacak şekilde kontrol edilir
            var isCorrect = !string.IsNullOrEmpty(request.SelectedAnswer) &&
                            request.SelectedAnswer.Equals(question.Data!.CorrectAnswer, StringComparison.OrdinalIgnoreCase);

            answer.SelectedAnswer = request.SelectedAnswer;
            answer.CreatedDate = DateTime.Now;
            answer.IsCorrect = isCorrect;

            answerRepository.Update(answer);
            await unitOfWork.SaveChangeAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
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
            await unitOfWork.SaveChangeAsync();
            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult<List<AnswerResponseDto>>> GetByUserAndExamAsync(int userId, int examId)
        {
            var answers = await answerRepository.GetByUserAndExam(userId, examId).ToListAsync();
            if (!answers.Any())
            {
                return ServiceResult<List<AnswerResponseDto>>.Fail("Answers not found", HttpStatusCode.NotFound);
            }

            var answerAsDto = answers.Select(a => new AnswerResponseDto(a.AnswerId, a.UserId, a.QuestionId, a.SelectedAnswer, a.IsCorrect)).ToList();
            return ServiceResult<List<AnswerResponseDto>>.Success(answerAsDto);
        }

    }
}
