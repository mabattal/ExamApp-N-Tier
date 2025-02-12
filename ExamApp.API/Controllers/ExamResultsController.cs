using ExamApp.Services.ExamResult;
using Microsoft.AspNetCore.Mvc;

namespace ExamApp.API.Controllers
{
    public class ExamResultsController(IExamResultService _examResultService) : CustomBaseController
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetExamResult(int id)
        {
            var result = await _examResultService.GetByIdAsync(id);
            return CreateActionResult(result);
        }

        [HttpGet("{userId}/{examId}")]
        public async Task<IActionResult> GetExamResultByUserIdAndExamId(int userId, int examId)
        {
            var result = await _examResultService.GetByUserIdAndExamId(userId, examId);
            return CreateActionResult(result);
        }

        [HttpPost("start/{examId}/{userId}")]
        public async Task<IActionResult> StartExam(int examId, int userId)
        {
            var result = await _examResultService.StartExamAsync(examId, userId);
            return CreateActionResult(result);
        }

        [HttpPost("submit/{examId}/{userId}")]
        public async Task<IActionResult> SubmitExam(int examId, int userId)
        {
            var result = await _examResultService.SubmitExamAsync(examId, userId);
            return CreateActionResult(result);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetExamResultsByUserId(int userId)
        {
            var result = await _examResultService.GetByUserIdAsync(userId);
            return CreateActionResult(result);
        }

        [HttpGet("average/{examId}")]
        public async Task<IActionResult> GetAverageScoreByExam(int examId)
        {
            var result = await _examResultService.GetAverageScoreByExamAsync(examId);
            return CreateActionResult(result);
        }
    }
}
