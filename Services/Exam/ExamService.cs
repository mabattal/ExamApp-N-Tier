using ExamApp.Repositories;
using ExamApp.Repositories.Repositories;
using ExamApp.Services.Exam.Create;
using ExamApp.Services.Exam.Update;
using ExamApp.Services.Question;
using ExamApp.Services.User;
using Microsoft.EntityFrameworkCore;
using System.Net;

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

            // Türkiye saat dilimini al
            TimeZoneInfo turkeyTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
            var startDateUtc = TimeZoneInfo.ConvertTimeToUtc(DateTime.SpecifyKind(examRequest.StartDate, DateTimeKind.Unspecified), turkeyTimeZone);
            var endDateUtc = TimeZoneInfo.ConvertTimeToUtc(DateTime.SpecifyKind(examRequest.EndDate, DateTimeKind.Unspecified), turkeyTimeZone);


            if (startDateUtc >= endDateUtc)
            {
                return ServiceResult<CreateExamResponseDto>.Fail("Start date cannot be greater than end date.", HttpStatusCode.BadRequest);
            }

            if (examRequest.Duration > (endDateUtc - startDateUtc).TotalMinutes)
            {
                return ServiceResult<CreateExamResponseDto>.Fail("Duration cannot be greater than the difference between start and end date.", HttpStatusCode.BadRequest);
            }

            if (startDateUtc < DateTime.UtcNow || endDateUtc < DateTime.UtcNow)
            {
                return ServiceResult<CreateExamResponseDto>.Fail("Start date and end date must be greater than the current UTC date.", HttpStatusCode.BadRequest);
            }

            var exam = new Repositories.Entities.Exam()
            {
                Title = examRequest.Title,
                Description = examRequest.Description,
                StartDate = startDateUtc,
                EndDate = endDateUtc,
                Duration = examRequest.Duration,
                CreatedBy = examRequest.CreatedBy,
                IsDeleted = false
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

            // Türkiye saat dilimini al
            TimeZoneInfo turkeyTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
            var startDateUtc = TimeZoneInfo.ConvertTimeToUtc(DateTime.SpecifyKind(examRequest.StartDate, DateTimeKind.Unspecified), turkeyTimeZone);
            var endDateUtc = TimeZoneInfo.ConvertTimeToUtc(DateTime.SpecifyKind(examRequest.EndDate, DateTimeKind.Unspecified), turkeyTimeZone);

            if (startDateUtc >= endDateUtc)
            {
                return ServiceResult.Fail("Start date cannot be greater than end date.", HttpStatusCode.BadRequest);
            }

            if (examRequest.Duration > (endDateUtc - startDateUtc).TotalMinutes)
            {
                return ServiceResult.Fail("Duration cannot be greater than the difference between start and end date.", HttpStatusCode.BadRequest);
            }

            if (startDateUtc < DateTime.UtcNow || endDateUtc < DateTime.UtcNow)
            {
                return ServiceResult.Fail("Start date and end date must be greater than the current UTC date.", HttpStatusCode.BadRequest);
            }

            exam.Title = examRequest.Title;
            exam.Description = examRequest.Description;
            exam.StartDate = startDateUtc;
            exam.EndDate = endDateUtc;
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
            exam.IsDeleted = true;

            examRepository.Update(exam);
            await unitOfWork.SaveChangeAsync();
            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult<List<ExamWithQuestionsResponseDto>>> GetByInstructorAsync(int instructorId)
        {
            var instructor = await userService.GetInstructorByIdAsync(instructorId);
            if (instructor.IsFail)
            {
                return ServiceResult<List<ExamWithQuestionsResponseDto>>.Fail(instructor.ErrorMessage!, instructor.Status);
            }

            var exams = await examRepository.GetByInstructor(instructorId).ToListAsync();
            var examAsDto = exams.Select(e =>
            {
                TimeZoneInfo turkeyTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
                var localStartDate = TimeZoneInfo.ConvertTimeFromUtc(e.StartDate, turkeyTimeZone);
                var localCompletionDate = TimeZoneInfo.ConvertTimeFromUtc(e.EndDate, turkeyTimeZone);

                return new ExamWithQuestionsResponseDto(
                    e.ExamId,
                    e.Title,
                    e.Description,
                    localStartDate,
                    localCompletionDate,
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
                );
            }).ToList();

            return ServiceResult<List<ExamWithQuestionsResponseDto>>.Success(examAsDto);
        }

        public async Task<ServiceResult<List<ExamWithInstructorResponseDto>>> GetActiveExamsAsync()
        {
            var exams = await examRepository.GetActiveExams().ToListAsync();
            if (!exams.Any())
            {
                return ServiceResult<List<ExamWithInstructorResponseDto>>.Fail("There is no active exam.", HttpStatusCode.NotFound);
            }

            var examAsDto = exams.Select(e =>
            {
                var localStartDate = TimeZoneInfo.ConvertTimeFromUtc(e.StartDate, TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time"));
                var localEndDate = TimeZoneInfo.ConvertTimeFromUtc(e.EndDate, TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time"));

                return new ExamWithInstructorResponseDto(
                    e.ExamId,
                    e.Title,
                    e.Description,
                    localStartDate,
                    localEndDate,
                    e.Duration,
                    new UserResponseDto(
                        e.Instructor.UserId,
                        e.Instructor.FullName,
                        e.Instructor.Email,
                        e.Instructor.Role,
                        e.Instructor.IsDeleted)
                );
            }).ToList();

            return ServiceResult<List<ExamWithInstructorResponseDto>>.Success(examAsDto);
        }

        public async Task<ServiceResult<ExamWithDetailsResponseDto?>> GetByIdAsync(int id)
        {
            var exam = await examRepository.GetExamWithDetailsAsync(id);
            if (exam is null)
            {
                return ServiceResult<ExamWithDetailsResponseDto?>.Fail("Exam not found", HttpStatusCode.NotFound);
            }

            var localStartDate = TimeZoneInfo.ConvertTimeFromUtc(exam.StartDate, TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time"));
            var localEndDate = TimeZoneInfo.ConvertTimeFromUtc(exam.EndDate, TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time"));

            var examAsDto = new ExamWithDetailsResponseDto(
                exam.ExamId,
                exam.Title,
                exam.Description,
                localStartDate,
                localEndDate,
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
