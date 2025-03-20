namespace ExamApp.Services.Exam.Create
{
    public record CreateExamRequestDto(
        string Title,
        string Description,
        DateTime StartDate,
        DateTime EndDate,
        int Duration
    );
}
