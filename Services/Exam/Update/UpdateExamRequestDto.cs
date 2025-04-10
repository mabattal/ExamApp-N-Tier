namespace ExamApp.Services.Exam.Update
{
    public record UpdateExamRequestDto(
        string Title,
        string Description,
        DateTimeOffset StartDate,
        DateTimeOffset EndDate,
        int Duration
    );
}
