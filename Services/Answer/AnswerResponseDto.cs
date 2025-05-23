﻿namespace ExamApp.Services.Answer
{
    public record AnswerResponseDto(int AnswerId, int UserId, int QuestionId, string SelectedAnswer, bool? IsCorrect, DateTimeOffset CreatedDate);

    //public record AnswerResponseDto
    //{
    //    public int AnswerId { get; init; }
    //    public int UserId { get; init; }
    //    public int QuestionId { get; init; }
    //    public string SelectedAnswer { get; init; } = string.Empty;
    //    public bool IsCorrect { get; init; }
    //}
}
