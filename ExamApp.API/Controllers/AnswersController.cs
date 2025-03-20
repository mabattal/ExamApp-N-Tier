using ExamApp.Services.Answer;
using ExamApp.Services.Answer.Create;
using ExamApp.Services.Answer.Update;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExamApp.API.Controllers
{
    [Authorize]
    public class AnswersController(IAnswerService answerService) : CustomBaseController
    {

        [HttpGet("{answerId:int}")]
        public async Task<IActionResult> GetById(int answerId)
        {
            var result = await answerService.GetByIdAsync(answerId);
            return CreateActionResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreateAnswerRequestDto createAnswerRequest)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var result = await answerService.AddAsync(createAnswerRequest, userId);
            return CreateActionResult(result);
        }

        [HttpPut("{answerId:int}")]
        public async Task<IActionResult> Update(int answerId, UpdateAnswerRequestDto updateAnswerRequest)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var result = await answerService.UpdateAsync(answerId, updateAnswerRequest, userId);
            return CreateActionResult(result);
        }

        [HttpDelete("{answerId:int}")]
        public async Task<IActionResult> Delete(int answerId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var result = await answerService.DeleteAsync(answerId, userId);
            return CreateActionResult(result);
        }

        [HttpGet("exam/{examId:int}")]
        public async Task<IActionResult> GetByUserAndExam(int examId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var result = await answerService.GetByUserAndExamAsync(userId, examId);
            return CreateActionResult(result);
        }

    }
}
