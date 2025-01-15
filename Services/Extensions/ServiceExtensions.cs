using ExamApp.Services.Services;
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


            return services;
        }
    }
}
