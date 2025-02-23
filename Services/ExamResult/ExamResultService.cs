using ExamApp.Repositories;
using ExamApp.Repositories.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Net;
using ExamApp.Services.Answer;
using ExamApp.Services.Exam;
using ExamApp.Services.Question;
using ExamApp.Services.User;

namespace ExamApp.Services.ExamResult
{
    public class ExamResultService(IExamResultRepository examResultRepository, IQuestionService questionService, IAnswerService answerService, IExamService examService, IUserService userService, IUnitOfWork unitOfWork) : IExamResultService
    {
        public async Task<ServiceResult<ExamResultResponseDto?>> GetByIdAsync(int id)
        {
            var examResult = await examResultRepository.GetByIdAsync(id);
            if (examResult is null)
            {
                return ServiceResult<ExamResultResponseDto>.Fail("Exam result not found.", HttpStatusCode.NotFound)!;
            }
            var examResultAsDto = new ExamResultResponseDto(examResult.ResultId, examResult.UserId, examResult.ExamId, examResult.Score, examResult.StartDate, examResult.CompletionDate, examResult.Duration, examResult.TotalQuestions, examResult.CorrectAnswers, examResult.IncorrectAnswers, examResult.EmptyAnswers);
            return ServiceResult<ExamResultResponseDto>.Success(examResultAsDto)!;
        }

        public async Task<ServiceResult<ExamResultResponseDto?>> GetByUserIdAndExamId(int userId, int examId)
        {
            var examResult = await examResultRepository.GetByUserIdAndExamId(userId, examId).SingleOrDefaultAsync();
            if (examResult is null)
            {
                return ServiceResult<ExamResultResponseDto>.Fail("Exam result not found.", HttpStatusCode.NotFound)!;
            }
            var examResultAsDto = new ExamResultResponseDto(examResult.ResultId, examResult.UserId, examResult.ExamId, examResult.Score, examResult.StartDate, examResult.CompletionDate, examResult.Duration, examResult.TotalQuestions, examResult.CorrectAnswers, examResult.IncorrectAnswers, examResult.EmptyAnswers);
            return ServiceResult<ExamResultResponseDto>.Success(examResultAsDto)!;
        }

        public async Task<ServiceResult> StartExamAsync(int examId, int userId)
        {
            var exam = await examService.GetByIdAsync(examId);
            if (exam.IsFail)
            {
                return ServiceResult.Fail(exam.ErrorMessage, exam.Status);
            }

            var questions = await questionService.GetByExamIdAsync(examId);
            if (questions.IsFail)
            {
                return ServiceResult.Fail(questions.ErrorMessage, questions.Status);
            }

            // Check if the exam has already been started by the user
            var existingResult = await examResultRepository.Where(x => x.ExamId == examId && x.UserId == userId).AnyAsync();
            if (existingResult)
            {
                return ServiceResult.Fail("Exam already started.", HttpStatusCode.BadRequest);
            }

            if (exam.Data!.StartDate > DateTime.Now)
            {
                return ServiceResult.Fail("Exam has not started yet.", HttpStatusCode.BadRequest);
            }

            if(exam.Data.EndDate < DateTime.Now)
            {
                return ServiceResult.Fail("Exam has already ended.", HttpStatusCode.BadRequest);
            }

            var examResult = new Repositories.Entities.ExamResult()
            {
                UserId = userId,
                ExamId = examId,
                StartDate = DateTime.Now,
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
            // Hiç cevap oluşturulmadan sınav bitirilmek istenirse
            var correctAnswers = 0;
            var incorrectAnswers = 0;
            var emptyAnswers = totalQuestions;
            decimal score = 0;
            var answers = await answerService.GetByUserAndExamAsync(userId, examId);
            if(answers.IsSuccess)
            {
                correctAnswers = answers.Data!.Count(a => a.IsCorrect == true);
                incorrectAnswers = answers.Data!.Count(a => a.IsCorrect == false);
                emptyAnswers = totalQuestions - (correctAnswers + incorrectAnswers);
                score = (correctAnswers / (decimal)totalQuestions) * 100;
            }
            var duration = (int)(DateTime.Now - existingResult.StartDate).TotalMinutes;

            existingResult.Score = score;
            existingResult.CompletionDate = DateTime.Now;
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
                            (x.StartDate.AddMinutes(x.Exam.Duration) <= DateTime.Now ||
                             (x.StartDate.AddMinutes(x.Exam.Duration) > x.Exam.EndDate && x.Exam.EndDate <= DateTime.Now)))
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
                    return submitResult;        //SubmitExamAsync servisinden dönen hatayı geri döndürdük
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

            var examResults = await examResultRepository.GetByUserId(userId).ToListAsync();
            if (!examResults.Any())
            {
                return ServiceResult<List<ExamResultResponseDto>>.Fail("No exam results found.", HttpStatusCode.NotFound);
            }
            var examResultsAsDto = examResults.Select(x => new ExamResultResponseDto(x.ResultId, x.UserId, x.ExamId, x.Score, x.StartDate, x.CompletionDate, x.Duration, x.TotalQuestions, x.CorrectAnswers, x.IncorrectAnswers, x.EmptyAnswers)).ToList();
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
