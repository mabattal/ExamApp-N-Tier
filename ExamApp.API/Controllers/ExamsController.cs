using ExamApp.Services.Exam;
using ExamApp.Services.Exam.Create;
using ExamApp.Services.Exam.Update;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExamApp.API.Controllers
{
    [Authorize]
    public class ExamsController(IExamService _examService) : CustomBaseController
    {
        [Authorize(Roles = "Instructor, Admin")]
        [HttpGet("instructor/{instructorId:int}")]
        public async Task<IActionResult> GetByInstructor()
        {
            var instructorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var result = await _examService.GetByInstructorAsync(instructorId);
            return CreateActionResult(result);
        }

        [Authorize(Roles = "Student, Instructor, Admin")]
        [HttpGet("active")]
        public async Task<IActionResult> GetActiveExams()
        {
            var result = await _examService.GetActiveExamsAsync();
            return CreateActionResult(result);
        }

        [Authorize(Roles = "Student, Instructor, Admin")]
        [HttpGet("{examId:int}")]
        public async Task<IActionResult> GetById(int examId)
        {
            var result = await _examService.GetByIdAsync(examId);
            return CreateActionResult(result);
        }

        [Authorize(Roles = "Instructor, Admin")]
        [HttpPost]
        public async Task<IActionResult> Add(CreateExamRequestDto createExamRequest)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var result = await _examService.AddAsync(createExamRequest, userId);
            return CreateActionResult(result);
        }

        [Authorize(Roles = "Instructor, Admin")]
        [HttpPut("{examId:int}")]
        public async Task<IActionResult> Update(int examId, UpdateExamRequestDto updateExamRequest)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var result = await _examService.UpdateAsync(examId, updateExamRequest, userId);
            return CreateActionResult(result);
        }

        [Authorize(Roles = "Instructor, Admin")]
        [HttpDelete("{examId:int}")]
        public async Task<IActionResult> Delete(int examId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var result = await _examService.DeleteAsync(examId, userId);
            return CreateActionResult(result);
        }
    }
}
