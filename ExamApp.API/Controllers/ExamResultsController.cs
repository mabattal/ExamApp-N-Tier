using ExamApp.Services.ExamResult;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExamApp.API.Controllers
{
    [Authorize]
    public class ExamResultsController(IExamResultService examResultService) : CustomBaseController
    {
        [Authorize(Roles = "Student, Instructor, Admin")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetExamResult(int id)
        {
            var result = await examResultService.GetByIdAsync(id);
            return CreateActionResult(result);
        }

        [Authorize(Roles = "Instructor, Admin")]
        [HttpGet("{userId:int}/{examId:int}")]
        public async Task<IActionResult> GetExamResultByUserIdAndExamId(int userId, int examId)
        {
            var result = await examResultService.GetByUserIdAndExamId(userId, examId);
            return CreateActionResult(result);
        }

        [Authorize(Roles = "Student")]
        [HttpPost("start/{examId:int}")]
        public async Task<IActionResult> StartExam(int examId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var result = await examResultService.StartExamAsync(examId, userId);
            return CreateActionResult(result);
        }

        [Authorize(Roles = "Student")]
        [HttpPost("submit/{examId:int}")]
        public async Task<IActionResult> SubmitExam(int examId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var result = await examResultService.SubmitExamAsync(examId, userId);
            return CreateActionResult(result);
        }

        [Authorize(Roles = "Student, Instructor, Admin")]
        [HttpGet("user/{userId:int}")]
        public async Task<IActionResult> GetExamResultsByUserId(int userId)
        {
            var result = await examResultService.GetByUserIdAsync(userId);
            return CreateActionResult(result);
        }

        [Authorize(Roles = "Instructor, Admin")]
        [HttpGet("average/{examId:int}")]
        public async Task<IActionResult> GetAverageScoreByExam(int examId)
        {
            var result = await examResultService.GetAverageScoreByExamAsync(examId);
            return CreateActionResult(result);
        }
    }
}
