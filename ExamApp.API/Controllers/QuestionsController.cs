using ExamApp.Services.Question;
using ExamApp.Services.Question.Create;
using ExamApp.Services.Question.Update;
using Microsoft.AspNetCore.Mvc;

namespace ExamApp.API.Controllers
{
    public class QuestionsController(IQuestionService _questionService) : CustomBaseController
    {
        [HttpGet("{questionId:int}")]
        public async Task<IActionResult> GetById(int questionId)
        {
            var result = await _questionService.GetByIdAsync(questionId);
            return CreateActionResult(result);
        }

        [HttpGet("examId/{examId:int}")]
        public async Task<IActionResult> GetByExamId(int examId)
        {
            var result = await _questionService.GetByExamIdAsync(examId);
            return CreateActionResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreateQuestionRequestDto createQuestionRequest)
        {
            var result = await _questionService.AddAsync(createQuestionRequest);
            return CreateActionResult(result);
        }

        [HttpPut("{questionId:int}")]
        public async Task<IActionResult> Update(int questionId, UpdateQuestionRequestDto updateQuestionRequest)
        {
            var result = await _questionService.UpdateAsync(questionId, updateQuestionRequest);
            return CreateActionResult(result);
        }

        [HttpDelete("{questionId:int}")]
        public async Task<IActionResult> Delete(int questionId)
        {
            var result = await _questionService.DeleteAsync(questionId);
            return CreateActionResult(result);
        }

    }
}
