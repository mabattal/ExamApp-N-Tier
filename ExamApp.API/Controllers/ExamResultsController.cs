using ExamApp.Services.ExamResult;
using Microsoft.AspNetCore.Mvc;

namespace ExamApp.API.Controllers
{
    public class ExamResultsController(IExamResultService _examResultService) : CustomBaseController
    {
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetExamResult(int id)
        {
            var result = await _examResultService.GetByIdAsync(id);
            return CreateActionResult(result);
        }

        [HttpGet("{userId:int}/{examId:int}")]
        public async Task<IActionResult> GetExamResultByUserIdAndExamId(int userId, int examId)
        {
            var result = await _examResultService.GetByUserIdAndExamId(userId, examId);
            return CreateActionResult(result);
        }

        [HttpPost("start/{examId:int}/{userId:int}")]
        public async Task<IActionResult> StartExam(int examId, int userId)
        {
            var result = await _examResultService.StartExamAsync(examId, userId);
            return CreateActionResult(result);
        }

        [HttpPost("submit/{examId:int}/{userId:int}")]
        public async Task<IActionResult> SubmitExam(int examId, int userId)
        {
            var result = await _examResultService.SubmitExamAsync(examId, userId);
            return CreateActionResult(result);
        }

        [HttpGet("user/{userId:int}")]
        public async Task<IActionResult> GetExamResultsByUserId(int userId)
        {
            var result = await _examResultService.GetByUserIdAsync(userId);
            return CreateActionResult(result);
        }

        [HttpGet("average/{examId:int}")]
        public async Task<IActionResult> GetAverageScoreByExam(int examId)
        {
            var result = await _examResultService.GetAverageScoreByExamAsync(examId);
            return CreateActionResult(result);
        }
    }
}
