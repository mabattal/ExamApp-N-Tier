using ExamApp.Services.Exam;
using ExamApp.Services.Exam.Create;
using ExamApp.Services.Exam.Update;
using Microsoft.AspNetCore.Mvc;

namespace ExamApp.API.Controllers
{
    public class ExamsController(IExamService _examService) : CustomBaseController
    {
        [HttpGet("instructor/{instructorId:int}")]
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

        [HttpGet("{examId:int}")]
        public async Task<IActionResult> GetById(int examId)
        {
            var result = await _examService.GetByIdAsync(examId);
            return CreateActionResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreateExamRequestDto createExamRequest)
        {
            var result = await _examService.AddAsync(createExamRequest);
            return CreateActionResult(result);
        }

        [HttpPut("{examId:int}")]
        public async Task<IActionResult> Update(int examId, UpdateExamRequestDto updateExamRequest)
        {
            var result = await _examService.UpdateAsync(examId, updateExamRequest);
            return CreateActionResult(result);
        }

        [HttpDelete("{examId:int}")]
        public async Task<IActionResult> Delete(int examId)
        {
            var result = await _examService.DeleteAsync(examId);
            return CreateActionResult(result);
        }
    }
}
