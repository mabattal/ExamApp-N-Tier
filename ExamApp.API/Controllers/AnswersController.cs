using ExamApp.Services.Answer;
using ExamApp.Services.Question;
using Microsoft.AspNetCore.Mvc;

namespace ExamApp.API.Controllers
{
    public class AnswersController(IAnswerService _answerService) : CustomBaseController
    {

        [HttpGet("{answerId}")]
        public async Task<IActionResult> GetById(int answerId)
        {
            var result = await _answerService.GetByIdAsync(answerId);
            return CreateActionResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreateAnswerRequestDto createAnswerRequest)
        {
            var result = await _answerService.AddAsync(createAnswerRequest);
            return CreateActionResult(result);
        }

        [HttpPut("{answerId}")]
        public async Task<IActionResult> Update(int answerId, UpdateAnswerRequestDto updateAnswerRequest)
        {
            var result = await _answerService.UpdateAsync(answerId, updateAnswerRequest);
            return CreateActionResult(result);
        }

        [HttpDelete("{answerId}")]
        public async Task<IActionResult> Delete(int answerId)
        {
            var result = await _answerService.DeleteAsync(answerId);
            return CreateActionResult(result);
        }

        [HttpGet("{userId}/{examId}")]
        public async Task<IActionResult> GetByUserAndExam(int userId, int examId)
        {
            var result = await _answerService.GetByUserAndExamAsync(userId, examId);
            return CreateActionResult(result);
        }

    }
}
