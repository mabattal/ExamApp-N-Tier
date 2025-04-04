﻿namespace ExamApp.Repositories.Answers
{
    public interface IAnswerRepository : IGenericRepository<Answer>
    {
        IQueryable<Answer> GetByUserAndExam(int userId, int examId);
        IQueryable<Answer?> GetByUserAndQuestion(int userId, int questionId);
        Task<Answer?> GetByIdWithDetailsAsync(int id);
    }
}
