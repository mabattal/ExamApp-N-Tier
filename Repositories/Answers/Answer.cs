﻿using ExamApp.Repositories.Questions;
using ExamApp.Repositories.Users;

namespace ExamApp.Repositories.Answers
{
    public class Answer : IAuditEntity
    {
        public int AnswerId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public string? SelectedAnswer { get; set; } = string.Empty;
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public bool? IsCorrect { get; set; }
    }
}
