namespace ExamApp.Services.Exam
{
    public record CreateExamRequestDto(
        string Title,
        string Description,
        DateTime StartDate,
        DateTime EndDate,
        int Duration, // Dakika cinsinden
        int CreatedBy, // Eğitmen ID
        ICollection<int> QuestionIds // Soruların ID'leri
    );
}
