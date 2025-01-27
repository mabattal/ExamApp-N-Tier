using System.Net;
using ExamApp.Repositories;
using ExamApp.Repositories.Repositories;
using ExamApp.Services.Question;
using Microsoft.EntityFrameworkCore;

namespace ExamApp.Services.Exam
{
    public class ExamService(IExamRepository examRepository, IUnitOfWork unitOfWork) : IExamService
    {
        public async Task<ServiceResult<CreateExamResponseDto>> AddAsync(CreateExamRequestDto examRequest)
        {
            var exam = new Repositories.Entities.Exam()
            {
                Title = examRequest.Title,
                Description = examRequest.Description,
                StartDate = examRequest.StartDate,
                EndDate = examRequest.EndDate,
                Duration = examRequest.Duration,
                CreatedBy = examRequest.CreatedBy
            };
            await examRepository.AddAsync(exam);
            await unitOfWork.SaveChangeAsync();

            return ServiceResult<CreateExamResponseDto>.Success(new CreateExamResponseDto(exam.ExamId));
        }

        public async Task<ServiceResult> UpdateAsync(int id, UpdateExamRequestDto examRequest)
        {
            var exam = await examRepository.GetByIdAsync(id);

            if (exam is null)
            {
                return ServiceResult.Fail("Exam not found", HttpStatusCode.NotFound);
            }

            exam.Title = examRequest.Title;
            exam.Description = examRequest.Description;
            exam.StartDate = examRequest.StartDate;
            exam.EndDate = examRequest.EndDate;
            exam.Duration = examRequest.Duration;

            examRepository.Update(exam);
            await unitOfWork.SaveChangeAsync();

            return ServiceResult.Success();
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var exam = await examRepository.GetByIdAsync(id);
            if (exam is null)
            {
                return ServiceResult.Fail("Exam not found", HttpStatusCode.NotFound);
            }
            examRepository.Delete(exam);
            await unitOfWork.SaveChangeAsync();
            return ServiceResult.Success();
        }

        public async Task<ServiceResult<List<ExamResponseDto>>> GetByInstructorAsync(int instructorId)
        {
            var exams = await examRepository.GetByInstructor(instructorId).ToListAsync();
            var examAsDto = exams.Select(e => new ExamResponseDto(e.ExamId, e.Title, e.Description, e.StartDate, e.EndDate, e.Duration, e.CreatedBy)).ToList();
            return ServiceResult<List<ExamResponseDto>>.Success(examAsDto);
        }

        public async Task<ServiceResult<List<ExamResponseDto>>> GetActiveExamsAsync()
        {
            var exams = await examRepository.GetActiveExams().ToListAsync();
            var examAsDto = exams.Select(e => new ExamResponseDto(e.ExamId, e.Title, e.Description, e.StartDate, e.EndDate, e.Duration, e.CreatedBy)).ToList();
            return ServiceResult<List<ExamResponseDto>>.Success(examAsDto);
        }

        public async Task<ServiceResult<ExamWithDetailsResponseDto?>> GetExamWithDetailsAsync(int examId)
        {
            var exam = await examRepository.GetExamWithDetailsAsync(examId);

            if (exam is null)
            {
                return ServiceResult<ExamWithDetailsResponseDto?>.Fail($"Exam with ID {examId} not found.", HttpStatusCode.NotFound);
            }

            // Sınav detaylarını DTO'ya dönüştür
            var examAsDto = new ExamWithDetailsResponseDto(
                exam.ExamId,
                exam.Title,
                exam.Description,
                exam.StartDate,
                exam.EndDate,
                exam.Duration,
                exam.CreatedBy,
                exam.Instructor!.Email,
                exam.Questions.Select(q => new QuestionResponseDto(
                    q.QuestionId,
                    q.QuestionText,
                    q.OptionA,
                    q.OptionB,
                    q.OptionC,
                    q.OptionD,
                    q.CorrectAnswer
                )).ToList()
            );

            return ServiceResult<ExamWithDetailsResponseDto?>.Success(examAsDto!);
        }
    }
}
