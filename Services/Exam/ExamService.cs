﻿using AutoMapper;
using ExamApp.Repositories;
using ExamApp.Repositories.Exams;
using ExamApp.Services.Exam.Create;
using ExamApp.Services.Exam.Update;
using ExamApp.Services.User;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ExamApp.Services.Exam
{
    public class ExamService(
        IExamRepository examRepository, 
        IUserService userService, 
        IUnitOfWork unitOfWork, 
        IDateTimeUtcConversionService dateTimeService,
        IMapper mapper) : IExamService
    {
        public async Task<ServiceResult<CreateExamResponseDto>> AddAsync(CreateExamRequestDto examRequest, int userId)
        {
            var instructor = await userService.GetInstructorByIdAsync(userId);
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

            if (examRequest.StartDate < DateTimeOffset.UtcNow || examRequest.EndDate < DateTimeOffset.UtcNow)
            {
                return ServiceResult<CreateExamResponseDto>.Fail("Start date and end date must be greater than the current date.", HttpStatusCode.BadRequest);
            }

            var exam = mapper.Map<Repositories.Exams.Exam>(examRequest);
            exam.CreatedBy = userId;
            exam.StartDate = dateTimeService.ConvertToUtc(examRequest.StartDate);
            exam.EndDate = dateTimeService.ConvertToUtc(examRequest.EndDate);
            exam.IsDeleted = false;

            await examRepository.AddAsync(exam);
            await unitOfWork.SaveChangeAsync();

            return ServiceResult<CreateExamResponseDto>.Success(new CreateExamResponseDto(exam.ExamId));
        }

        public async Task<ServiceResult> UpdateAsync(int id, UpdateExamRequestDto examRequest, int userId)
        {
            var exam = await examRepository.GetByIdAsync(id);
            if (exam is null)
            {
                return ServiceResult.Fail("Exam not found", HttpStatusCode.NotFound);
            }

            var instructor = await userService.GetInstructorByIdAsync(userId);
            if (instructor.IsFail)
            {
                return ServiceResult.Fail(instructor.ErrorMessage!, instructor.Status);
            }

            if (exam.CreatedBy != userId)
            {
                return ServiceResult.Fail("You are not authorized to update this exam.", HttpStatusCode.Forbidden);
            }

            if (examRequest.StartDate >= examRequest.EndDate)
            {
                return ServiceResult.Fail("Start date cannot be greater than end date.", HttpStatusCode.BadRequest);
            }

            if (examRequest.Duration > (examRequest.EndDate - examRequest.StartDate).TotalMinutes)
            {
                return ServiceResult.Fail("Duration cannot be greater than the difference between start and end date.", HttpStatusCode.BadRequest);
            }

            if (examRequest.StartDate < DateTimeOffset.UtcNow || examRequest.EndDate < DateTimeOffset.UtcNow)
            {
                return ServiceResult.Fail("Start date and end date must be greater than the current date.", HttpStatusCode.BadRequest);
            }

            mapper.Map(examRequest, exam);
            exam.StartDate = dateTimeService.ConvertToUtc(examRequest.StartDate);
            exam.EndDate = dateTimeService.ConvertToUtc(examRequest.EndDate);

            examRepository.Update(exam);
            await unitOfWork.SaveChangeAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult> DeleteAsync(int id, int userId)
        {
            var exam = await examRepository.GetByIdAsync(id);
            if (exam is null)
            {
                return ServiceResult.Fail("Exam not found", HttpStatusCode.NotFound);
            }
            if (exam.CreatedBy != userId)
            {
                return ServiceResult.Fail("You are not authorized to delete this exam.", HttpStatusCode.Forbidden);
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
                e.StartDate = dateTimeService.ConvertFromUtc(e.StartDate);
                e.EndDate = dateTimeService.ConvertFromUtc(e.EndDate);
                var dto = mapper.Map<ExamWithQuestionsResponseDto>(e);
                e.StartDate = dateTimeService.ConvertToUtc(e.StartDate);
                e.EndDate = dateTimeService.ConvertToUtc(e.EndDate);
                return dto;
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
                e.StartDate = dateTimeService.ConvertFromUtc(e.StartDate);
                e.EndDate = dateTimeService.ConvertFromUtc(e.EndDate);
                var dto = mapper.Map<ExamWithInstructorResponseDto>(e);
                e.StartDate = dateTimeService.ConvertToUtc(e.StartDate);
                e.EndDate = dateTimeService.ConvertToUtc(e.EndDate);
                return dto;
            }).ToList();

            return ServiceResult<List<ExamWithInstructorResponseDto>>.Success(examAsDto);
        }

        public async Task<ServiceResult<List<ExamWithInstructorResponseDto>>> GetPastExamsAsync()
        {
            var exams = await examRepository.GetPastExams().ToListAsync();
            if (!exams.Any())
            {
                return ServiceResult<List<ExamWithInstructorResponseDto>>.Fail("There is no past exam.", HttpStatusCode.NotFound);
            }

            var examAsDto = exams.Select(e =>
            {
                e.StartDate = dateTimeService.ConvertFromUtc(e.StartDate);
                e.EndDate = dateTimeService.ConvertFromUtc(e.EndDate);
                var dto = mapper.Map<ExamWithInstructorResponseDto>(e);
                e.StartDate = dateTimeService.ConvertToUtc(e.StartDate);
                e.EndDate = dateTimeService.ConvertToUtc(e.EndDate);
                return dto;
            }).ToList();

            return ServiceResult<List<ExamWithInstructorResponseDto>>.Success(examAsDto);
        }

        public async Task<ServiceResult<List<ExamWithInstructorResponseDto>>> GetUpcomingExamsAsync()
        {
            var exams = await examRepository.GetUpcomingExams().ToListAsync();
            if (!exams.Any())
            {
                return ServiceResult<List<ExamWithInstructorResponseDto>>.Fail("There is no upcoming exam.", HttpStatusCode.NotFound);
            }

            var examAsDto = exams.Select(e =>
            {
                e.StartDate = dateTimeService.ConvertFromUtc(e.StartDate);
                e.EndDate = dateTimeService.ConvertFromUtc(e.EndDate);
                var dto = mapper.Map<ExamWithInstructorResponseDto>(e);
                e.StartDate = dateTimeService.ConvertToUtc(e.StartDate);
                e.EndDate = dateTimeService.ConvertToUtc(e.EndDate);
                return dto;
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

            exam.StartDate = dateTimeService.ConvertFromUtc(exam.StartDate);
            exam.EndDate = dateTimeService.ConvertFromUtc(exam.EndDate);
            var examAsDto = mapper.Map<ExamWithDetailsResponseDto>(exam);
            exam.StartDate = dateTimeService.ConvertToUtc(exam.StartDate);
            exam.EndDate = dateTimeService.ConvertToUtc(exam.EndDate);

            return ServiceResult<ExamWithDetailsResponseDto?>.Success(examAsDto);
        }
    }
}
