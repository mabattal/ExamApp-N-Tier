using ExamApp.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Net;
using AutoMapper;
using ExamApp.Services.Answer;
using ExamApp.Services.Exam;
using ExamApp.Services.Question;
using ExamApp.Services.User;
using ExamApp.Repositories.ExamResults;

namespace ExamApp.Services.ExamResult
{
    public class ExamResultService(
        IExamResultRepository examResultRepository,
        IQuestionService questionService,
        IAnswerService answerService,
        IExamService examService,
        IUserService userService,
        IUnitOfWork unitOfWork,
        IDateTimeUtcConversionService dateTimeService,
        IMapper mapper) : IExamResultService
    {
        public async Task<ServiceResult<ExamResultResponseDto?>> GetByIdAsync(int id)
        {
            var examResult = await examResultRepository.GetByIdAsync(id);
            if (examResult is null)
            {
                return ServiceResult<ExamResultResponseDto>.Fail("Exam result not found.", HttpStatusCode.NotFound)!;
            }

            if (examResult.CompletionDate is null)
            {
                return ServiceResult<ExamResultResponseDto>.Fail("Exam result has not been submitted yet.", HttpStatusCode.BadRequest)!;
            }

            examResult.StartDate = dateTimeService.ConvertFromUtc(examResult.StartDate);
            examResult.CompletionDate = dateTimeService.ConvertFromUtc(examResult.CompletionDate.Value);
            var examResultAsDto = mapper.Map<ExamResultResponseDto>(examResult);
            examResult.StartDate = dateTimeService.ConvertToUtc(examResult.StartDate);
            examResult.CompletionDate = dateTimeService.ConvertToUtc(examResult.CompletionDate.Value);

            return ServiceResult<ExamResultResponseDto>.Success(examResultAsDto)!;
        }

        public async Task<ServiceResult<ExamResultResponseDto?>> GetByUserIdAndExamId(int userId, int examId)
        {
            var examResult = await examResultRepository.GetByUserIdAndExamId(userId, examId).SingleOrDefaultAsync();
            if (examResult is null)
            {
                return ServiceResult<ExamResultResponseDto>.Fail("Exam result not found.", HttpStatusCode.NotFound)!;
            }

            if (examResult.CompletionDate is null)
            {
                return ServiceResult<ExamResultResponseDto>.Fail("Exam result has not been submitted yet.", HttpStatusCode.BadRequest)!;
            }

            examResult.StartDate = dateTimeService.ConvertFromUtc(examResult.StartDate);
            examResult.CompletionDate = dateTimeService.ConvertFromUtc(examResult.CompletionDate.Value);
            var examResultAsDto = mapper.Map<ExamResultResponseDto>(examResult);
            examResult.StartDate = dateTimeService.ConvertToUtc(examResult.StartDate);
            examResult.CompletionDate = dateTimeService.ConvertToUtc(examResult.CompletionDate.Value);

            return ServiceResult<ExamResultResponseDto>.Success(examResultAsDto)!;
        }

        public async Task<ServiceResult<ExamResultResponseDto?>> GetByUserAndExam(int userId, int examId)
        {
            var examResult = await examResultRepository.GetByUserIdAndExamId(userId, examId).SingleOrDefaultAsync();
            if (examResult is null)
            {
                return ServiceResult<ExamResultResponseDto>.Fail("Exam result not found.", HttpStatusCode.NotFound)!;
            }

            var examResultAsDto = mapper.Map<ExamResultResponseDto>(examResult);
            return ServiceResult<ExamResultResponseDto>.Success(examResultAsDto)!;
        }

        public async Task<ServiceResult> StartExamAsync(int examId, int userId)
        {
            var exam = await examService.GetByIdAsync(examId);
            if (exam.IsFail)
            {
                return ServiceResult.Fail(exam.ErrorMessage!, exam.Status);
            }

            if (exam.Data!.StartDate > DateTimeOffset.UtcNow)
            {
                return ServiceResult.Fail("Exam has not started yet.", HttpStatusCode.BadRequest);
            }

            if (exam.Data.EndDate < DateTimeOffset.UtcNow)
            {
                return ServiceResult.Fail("Exam has already ended.", HttpStatusCode.BadRequest);
            }

            var existingResult = await examResultRepository.Where(x => x.ExamId == examId && x.UserId == userId).AnyAsync();
            if (existingResult)
            {
                return ServiceResult.Fail("Exam already started.", HttpStatusCode.BadRequest);
            }

            var questions = await questionService.GetByExamIdAsync(examId);
            if (questions.IsFail)
            {
                return ServiceResult.Fail(questions.ErrorMessage!, questions.Status);
            }


            var examResult = new Repositories.ExamResults.ExamResult()
            {
                UserId = userId,
                ExamId = examId,
                StartDate = DateTimeOffset.UtcNow,
                TotalQuestions = questions.Data!.Count
            };
            await examResultRepository.AddAsync(examResult);
            await unitOfWork.SaveChangeAsync();
            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult> SubmitExamAsync(int examId, int userId)
        {
            var exam = await examService.GetByIdAsync(examId);
            if (exam.IsFail)
            {
                return ServiceResult.Fail(exam.ErrorMessage!, exam.Status);
            }

            var existingResult = await examResultRepository.Where(x => x.ExamId == examId && x.UserId == userId).SingleOrDefaultAsync();
            if (existingResult is null)
            {
                return ServiceResult.Fail("Exam result is not found.", HttpStatusCode.NotFound);
            }

            if (existingResult.Score != null && existingResult.CompletionDate != null &&
                existingResult.CorrectAnswers != null && existingResult.IncorrectAnswers != null &&
                existingResult.Duration != null)
            {
                return ServiceResult.Fail("Exam has already been submitted.", HttpStatusCode.BadRequest);
            }

            var questions = await questionService.GetByExamIdAsync(examId);
            if (questions.IsFail)
            {
                return ServiceResult.Fail(questions.ErrorMessage!, questions.Status);
            }

            var totalQuestions = questions.Data!.Count;
            var correctAnswers = 0;
            var incorrectAnswers = 0;
            var emptyAnswers = totalQuestions;
            decimal score = 0;
            var answers = await answerService.GetByUserAndExamAsync(userId, examId);
            if (answers.IsSuccess)
            {
                correctAnswers = answers.Data!.Count(a => a.IsCorrect == true);
                incorrectAnswers = answers.Data!.Count(a => a.IsCorrect == false);
                emptyAnswers = totalQuestions - (correctAnswers + incorrectAnswers);
                score = (correctAnswers / (decimal)totalQuestions) * 100;
            }
            var duration = (int)(DateTimeOffset.UtcNow - existingResult.StartDate).TotalMinutes;

            existingResult.Score = score;
            existingResult.CompletionDate = DateTimeOffset.UtcNow;
            existingResult.Duration = duration;
            existingResult.CorrectAnswers = correctAnswers;
            existingResult.IncorrectAnswers = incorrectAnswers;
            existingResult.EmptyAnswers = emptyAnswers;

            examResultRepository.Update(existingResult);
            await unitOfWork.SaveChangeAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult> AutoSubmitExpiredExamsAsync()
        {
            var expiredResults = await examResultRepository
                .Where(x => x.CompletionDate == null &&
                            (x.StartDate.AddMinutes(x.Exam.Duration) <= DateTimeOffset.UtcNow ||
                             (x.StartDate.AddMinutes(x.Exam.Duration) > x.Exam.EndDate && x.Exam.EndDate <= DateTimeOffset.UtcNow)))
                .Include(x => x.Exam)
                .ToListAsync();

            if (!expiredResults.Any())
            {
                return ServiceResult.Fail("No expired exams found.");
            }

            foreach (var result in expiredResults)
            {
                var submitResult = await SubmitExamAsync(result.ExamId, result.UserId);
                if (submitResult.IsFail)
                {
                    return submitResult;
                }
            }

            return ServiceResult.Success();
        }

        public async Task<ServiceResult<List<ExamResultResponseDto>>> GetByUserIdAsync(int userId)
        {
            var user = await userService.GetByIdOrEmailAsync(userId.ToString());
            if (user.IsFail)
            {
                return ServiceResult<List<ExamResultResponseDto>>.Fail(user.ErrorMessage!, user.Status);
            }

            var examResults = await examResultRepository.Where((er => er.UserId == userId && er.CompletionDate != null)).ToListAsync();
            if (!examResults.Any())
            {
                return ServiceResult<List<ExamResultResponseDto>>.Fail("No completed exam results found.", HttpStatusCode.NotFound);
            }
            
            var examResultsAsDto = examResults
                .Where(x => x.CompletionDate != null)
                .Select(x =>
            {
                x.StartDate = dateTimeService.ConvertFromUtc(x.StartDate);
                x.CompletionDate = dateTimeService.ConvertFromUtc(x.CompletionDate!.Value);
                var dto = mapper.Map<ExamResultResponseDto>(x);
                x.StartDate = dateTimeService.ConvertToUtc(x.StartDate);
                x.CompletionDate = dateTimeService.ConvertToUtc(x.CompletionDate!.Value);
                return dto;
            }).ToList();

            return ServiceResult<List<ExamResultResponseDto>>.Success(examResultsAsDto);
        }

        public async Task<ServiceResult<ExamResultAverageScoreResponseDto>> GetAverageScoreByExamAsync(int examId)
        {
            var examResults = await examResultRepository.Where(x => x.ExamId == examId).AnyAsync();
            if (!examResults)
            {
                return ServiceResult<ExamResultAverageScoreResponseDto>.Fail("No exam results found.", HttpStatusCode.NotFound);
            }

            var averageScore = examResultRepository.GetAverageScoreByExamAsync(examId);

            return ServiceResult<ExamResultAverageScoreResponseDto>.Success(new ExamResultAverageScoreResponseDto(averageScore.Result));
        }
    }
}
