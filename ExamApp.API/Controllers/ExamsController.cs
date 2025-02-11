﻿using ExamApp.Services.Exam;
using Microsoft.AspNetCore.Mvc;

namespace ExamApp.API.Controllers
{
    public class ExamsController(IExamService _examService) : CustomBaseController
    {
        [HttpGet("instructor/{instructorId}")]
        public async Task<IActionResult> GetByInstructor(int instructorId)
        {
            var result = await _examService.GetByInstructorAsync(instructorId);
            return CreateActionResult(result);
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveExams()
        {
            var result = await _examService.GetActiveExamsAsync();
            return CreateActionResult(result);
        }

        [HttpGet("{examId}")]
        public async Task<IActionResult> GetExamWithDetails(int examId)
        {
            var result = await _examService.GetExamWithDetailsAsync(examId);
            return CreateActionResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreateExamRequestDto createExamRequest)
        {
            var result = await _examService.AddAsync(createExamRequest);
            return CreateActionResult(result);
        }

        [HttpPut("{examId}")]
        public async Task<IActionResult> Update(int examId, UpdateExamRequestDto updateExamRequest)
        {
            var result = await _examService.UpdateAsync(examId, updateExamRequest);
            return CreateActionResult(result);
        }

        [HttpDelete("{examId}")]
        public async Task<IActionResult> Delete(int examId)
        {
            var result = await _examService.DeleteAsync(examId);
            return CreateActionResult(result);
        }
    }
}
