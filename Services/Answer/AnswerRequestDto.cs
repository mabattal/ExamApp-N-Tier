namespace ExamApp.Services.Answer
{
    //C# 9.0 ile gelecek olan init accessor'ı ile sadece constructor içerisinde set edilebilir property'ler tanımlanabilir.
    public record AnswerRequestDto(
        int UserId,
        int QuestionId,
        string SelectedAnswer,
        bool IsCorrect
        );

    //diğer bir kullanım şekli
    //public record AnswerRequestDto
    //{
    //    public int UserId { get; init; }
    //    public int QuestionId { get; init; }
    //    public string SelectedAnswer { get; init; } = string.Empty;
    //    public bool IsCorrect { get; init; }
    //}
}
