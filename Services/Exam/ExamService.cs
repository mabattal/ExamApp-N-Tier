using System.Net;
using ExamApp.Repositories;
using ExamApp.Repositories.Repositories;
using ExamApp.Services.Question;
using ExamApp.Services.User;
using Microsoft.EntityFrameworkCore;

namespace ExamApp.Services.Exam
{
    public class ExamService(IExamRepository examRepository, IUserService userService, IUnitOfWork unitOfWork) : IExamService
    {
        public async Task<ServiceResult<CreateExamResponseDto>> AddAsync(CreateExamRequestDto examRequest)
        {
            var instructor = await userService.GetInstructorByIdAsync(examRequest.CreatedBy);
            if (instructor.IsFail)
            {
                return ServiceResult<CreateExamResponseDto>.Fail(instructor.ErrorMessage!, instructor.Status);
            }

            if (examRequest.StartDate >= examRequest.EndDate)
            {
                return ServiceResult<CreateExamResponseDto>.Fail("Start date cannot be greater than end date.", HttpStatusCode.BadRequest);
            }

            if (examRequest.Duration > (examRequest.EndDate - examRequest.StartDate).TotalMinutes)
            {
                return ServiceResult<CreateExamResponseDto>.Fail("Duration cannot be greater than the difference between start and end date.", HttpStatusCode.BadRequest);
            }

            if (examRequest.StartDate < DateTime.Now || examRequest.EndDate < DateTime.Now)
            {
                return ServiceResult<CreateExamResponseDto>.Fail("Start date and end date must be greater than the current date.", HttpStatusCode.BadRequest);
            }

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

            var instructor = await userService.GetInstructorByIdAsync(examRequest.CreatedBy);
            if (instructor.IsFail)
            {
                return ServiceResult.Fail(instructor.ErrorMessage!, instructor.Status);
            }

            if (exam.CreatedBy != examRequest.CreatedBy)
            {
                return ServiceResult.Fail("You are not authorized to update this exam.", HttpStatusCode.Forbidden);
            }

            if (examRequest.StartDate > examRequest.EndDate)
            {
                return ServiceResult.Fail("Start date cannot be greater than end date.", HttpStatusCode.BadRequest);
            }

            if (examRequest.Duration > (examRequest.EndDate - examRequest.StartDate).TotalMinutes)
            {
                return ServiceResult.Fail("Duration cannot be greater than the difference between start and end date.", HttpStatusCode.BadRequest);
            }

            if (examRequest.StartDate < DateTime.Now || examRequest.EndDate < DateTime.Now)
            {
                return ServiceResult.Fail("Start date and end date must be greater than the current date.", HttpStatusCode.BadRequest);
            }

            exam.Title = examRequest.Title;
            exam.Description = examRequest.Description;
            exam.StartDate = examRequest.StartDate;
            exam.EndDate = examRequest.EndDate;
            exam.Duration = examRequest.Duration;

            examRepository.Update(exam);
            await unitOfWork.SaveChangeAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
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
            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult<List<ExamWithQuestionsResponseDto>>> GetByInstructorAsync(int instructorId)
        {
            var exams = await examRepository.GetByInstructor(instructorId).ToListAsync();
            var examAsDto = exams.Select(e => new ExamWithQuestionsResponseDto(
                e.ExamId,
                e.Title,
                e.Description,
                e.StartDate,
                e.EndDate,
                e.Duration,
                e.Questions.Select(q => new QuestionResponseDto(
                    q.QuestionId,
                    q.ExamId,
                    q.QuestionText,
                    q.OptionA,
                    q.OptionB,
                    q.OptionC,
                    q.OptionD,
                    q.CorrectAnswer
                )).ToList()
            )).ToList();
            return ServiceResult<List<ExamWithQuestionsResponseDto>>.Success(examAsDto);
        }

        public async Task<ServiceResult<List<ExamWithInstructorResponseDto>>> GetActiveExamsAsync()
        {
            var exams = await examRepository.GetActiveExams().ToListAsync();
            if (!exams.Any())
            {
                return ServiceResult<List<ExamWithInstructorResponseDto>>.Fail("There is no active exam.", HttpStatusCode.NotFound);
            }

            var examAsDto = exams.Select(e => new ExamWithInstructorResponseDto(
                e.ExamId,
                e.Title,
                e.Description,
                e.StartDate,
                e.EndDate,
                e.Duration,
                new UserResponseDto(
                    e.Instructor.UserId,
                    e.Instructor.FullName,
                    e.Instructor.Email,
                    e.Instructor.Role,
                    e.Instructor.IsDeleted)
            )).ToList();
            return ServiceResult<List<ExamWithInstructorResponseDto>>.Success(examAsDto);
        }

        public async Task<ServiceResult<ExamWithDetailsResponseDto?>> GetByIdAsync(int id)
        {
            var exam = await examRepository.GetExamWithDetailsAsync(id);
            if (exam is null)
            {
                return ServiceResult<ExamWithDetailsResponseDto?>.Fail("Exam not found", HttpStatusCode.NotFound);
            }
            var examAsDto = new ExamWithDetailsResponseDto(
                exam.ExamId,
                exam.Title,
                exam.Description,
                exam.StartDate,
                exam.EndDate,
                exam.Duration,
                new UserResponseDto(
                    exam.Instructor.UserId,
                    exam.Instructor.FullName,
                    exam.Instructor.Email,
                    exam.Instructor.Role,
                    exam.Instructor.IsDeleted),
                exam.Questions.Select(q => new QuestionResponseDto(
                    q.QuestionId,
                    q.ExamId,
                    q.QuestionText,
                    q.OptionA,
                    q.OptionB,
                    q.OptionC,
                    q.OptionD,
                    q.CorrectAnswer
                )).ToList()
            );
            return ServiceResult<ExamWithDetailsResponseDto?>.Success(examAsDto);
        }
    }
}
