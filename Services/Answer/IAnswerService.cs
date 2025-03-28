﻿using ExamApp.Services.Answer.Create;
using ExamApp.Services.Answer.Update;

namespace ExamApp.Services.Answer
{
    public interface IAnswerService
    {
        Task<ServiceResult<AnswerResponseDto?>> GetByIdAsync(int id);
        Task<ServiceResult<CreateAnswerResponseDto>> AddAsync(CreateAnswerRequestDto createAnswerRequest, int userId);
        Task<ServiceResult> UpdateAsync(int id, UpdateAnswerRequestDto updateAnswerRequest, int userId);
        Task<ServiceResult> DeleteAsync(int id, int userId);
        Task<ServiceResult<List<AnswerResponseDto>>> GetByUserAndExamAsync(int userId, int examId);     //bir kullanıcının bir sınavdaki tüm cevaplarını getir
    }
}
