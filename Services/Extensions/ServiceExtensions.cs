﻿using System.Reflection;
using ExamApp.Services.Answer;
using ExamApp.Services.Authentication;
using ExamApp.Services.Exam;
using ExamApp.Services.ExamResult;
using ExamApp.Services.Question;
using ExamApp.Services.User;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExamApp.Services.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAnswerService, AnswerService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IExamService, ExamService>();
            services.AddScoped<IExamResultService, ExamResultService>();
            services.AddScoped(sp => new Lazy<IExamResultService>(() => sp.GetRequiredService<IExamResultService>()));
            services.AddHostedService<ExamExpirationBackgroundService>();

            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddScoped<IDateTimeUtcConversionService, DateTimeUtcConversionService>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<JwtService>();
            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}
