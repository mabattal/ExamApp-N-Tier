namespace ExamApp.Services.Exam
{
    public record UpdateExamRequestDto(
        int ExamId,
        string Title,
        string Description,
        DateTime StartDate,
        DateTime EndDate,
        int Duration, // Dakika cinsinden
        ICollection<int> QuestionIds // Güncellenmiş soru ID'leri
    );
}
