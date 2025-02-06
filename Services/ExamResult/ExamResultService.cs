using ExamApp.Repositories;
using ExamApp.Repositories.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ExamApp.Services.ExamResult
{
    public class ExamResultService(IExamResultRepository examResultRepository, IQuestionRepository questionRepository, IAnswerRepository answerRepository, IExamRepository examRepository, IUnitOfWork unitOfWork) : IExamResultService
    {
        public async Task<ServiceResult<ExamResultResponseDto?>> GetByIdAsync(int id)
        {
            var examResult = await examResultRepository.GetByIdAsync(id);
            if (examResult is null)
            {
                return ServiceResult<ExamResultResponseDto>.Fail("Exam result not found.", HttpStatusCode.NotFound)!;
            }
            var examResultAsDto = new ExamResultResponseDto(examResult.ResultId, examResult.UserId, examResult.ExamId, examResult.Score, examResult.StartDate, examResult.CompletionDate, examResult.Duration, examResult.TotalQuestions, examResult.CorrectAnswers, examResult.IncorrectAnswers);
            return ServiceResult<ExamResultResponseDto>.Success(examResultAsDto)!;
        }

        public async Task<ServiceResult> StartExamAsync(int examId, int userId)
        {
            var exam = await examRepository.GetByIdAsync(examId);
            if (exam is null)
            {
                return ServiceResult.Fail("Exam not found.", HttpStatusCode.NotFound);
            }

            var questions = await questionRepository.GetByExamId(examId).ToListAsync();
            if (!questions.Any())
            {
                return ServiceResult.Fail("No questions found for the given exam.", HttpStatusCode.NotFound);
            }

            // Check if the exam has already been started by the user
            var existingResult = await examResultRepository.Where(x => x.ExamId == examId && x.UserId == userId).SingleOrDefaultAsync();
            if (existingResult is not null)
            {
                return ServiceResult.Fail("Exam already started.", HttpStatusCode.BadRequest);
            }

            if (exam.StartDate > DateTime.Now)
            {
                return ServiceResult.Fail("Exam has not started yet.", HttpStatusCode.BadRequest);
            }

            var examResult = new Repositories.Entities.ExamResult()
            {
                UserId = userId,
                ExamId = examId,
                StartDate = DateTime.Now,
                TotalQuestions = questions.Count
            };
            await examResultRepository.AddAsync(examResult);
            await unitOfWork.SaveChangeAsync();
            return ServiceResult.Success();
        }

        public async Task<ServiceResult> SubmitExamAsync(int examId, int userId)
        {
            var exam = await examRepository.GetByIdAsync(examId);
            if (exam is null)
            {
                return ServiceResult.Fail("Exam not found.", HttpStatusCode.NotFound);
            }

            var existingResult = await examResultRepository.Where(x => x.ExamId == examId && x.UserId == userId).SingleOrDefaultAsync();
            if (existingResult is null)
            {
                return ServiceResult.Fail("Exam result is not found.", HttpStatusCode.NotFound);
            }

            var questions = await questionRepository.GetByExamId(examId).ToListAsync();
            if (!questions.Any())
            {
                return ServiceResult.Fail("No questions found for the given exam.", HttpStatusCode.NotFound);
            }

            var answers = await answerRepository.GetByUserAndExam(examId, userId).ToListAsync();
            var correctAnswers = answers.Count(a => a.IsCorrect);
            var totalQuestions = questions.Count;
            var incorrectAnswers = totalQuestions - correctAnswers;
            var score = (correctAnswers / (double)totalQuestions) * 100;
            var duration = (int)(DateTime.Now - existingResult.StartDate).TotalMinutes;

            var examResult = new Repositories.Entities.ExamResult()
            {
                Score = score,
                CompletionDate = DateTime.Now,
                Duration = duration,
                CorrectAnswers = correctAnswers,
                IncorrectAnswers = incorrectAnswers
            };

            await examResultRepository.AddAsync(examResult);
            await unitOfWork.SaveChangeAsync();

            return ServiceResult.Success();
        }

        public async Task<ServiceResult> AutoSubmitExpiredExamsAsync()
        {
            var expiredResults = await examResultRepository
                .Where(x => x.CompletionDate == null && DateTime.UtcNow > x.StartDate.AddMinutes(x.Exam.Duration))
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

            var examResults = await examResultRepository.Where(x => x.UserId == userId).ToListAsync();
            if (!examResults.Any())
            {
                return ServiceResult<List<ExamResultResponseDto>>.Fail("No exam results found.", HttpStatusCode.NotFound);
            }
            var examResultsAsDto = examResults.Select(x => new ExamResultResponseDto(x.ResultId, x.UserId, x.ExamId, x.Score, x.StartDate, x.CompletionDate, x.Duration, x.TotalQuestions, x.CorrectAnswers, x.IncorrectAnswers)).ToList();
            return ServiceResult<List<ExamResultResponseDto>>.Success(examResultsAsDto);
        }

        public async Task<ServiceResult<ExamResultAverageScoreResponseDto>> GetAverageScoreByExamAsync(int examId)
        {
            var examResults = await examResultRepository.Where(x => x.ExamId == examId).ToListAsync();
            if (!examResults.Any())
            {
                return ServiceResult<ExamResultAverageScoreResponseDto>.Fail("No exam results found.", HttpStatusCode.NotFound);
            }

            var averageScore = examResultRepository.GetAverageScoreByExamAsync(examId);
            
            return ServiceResult<ExamResultAverageScoreResponseDto>.Success(new ExamResultAverageScoreResponseDto(averageScore.Result));
        }

    }
}
