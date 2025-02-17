using ExamApp.Services.Answer;
using Microsoft.AspNetCore.Mvc;

namespace ExamApp.API.Controllers
{
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
            var result = await answerService.AddAsync(createAnswerRequest);
            return CreateActionResult(result);
        }

        [HttpPut("{answerId:int}")]
        public async Task<IActionResult> Update(int answerId, UpdateAnswerRequestDto updateAnswerRequest)
        {
            var result = await answerService.UpdateAsync(answerId, updateAnswerRequest);
            return CreateActionResult(result);
        }

        [HttpDelete("{answerId:int}")]
        public async Task<IActionResult> Delete(int answerId)
        {
            var result = await answerService.DeleteAsync(answerId);
            return CreateActionResult(result);
        }

        [HttpGet("{userId:int}/{examId:int}")]
        public async Task<IActionResult> GetByUserAndExam(int userId, int examId)
        {
            var result = await answerService.GetByUserAndExamAsync(userId, examId);
            return CreateActionResult(result);
        }

    }
}
