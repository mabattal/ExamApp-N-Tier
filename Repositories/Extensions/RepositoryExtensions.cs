using ExamApp.Repositories.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExamApp.Repositories.Extensions
{
    public static class RepositoryExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                var connectionStrings = configuration.GetSection(ConnectionStringOption.Key).Get<ConnectionStringOption>();

                options.UseSqlServer(connectionStrings!.SqlServer, sqlServerOptionsAction =>
                {
                    sqlServerOptionsAction.MigrationsAssembly(typeof(RepositoryAssembly).Assembly.FullName);
                });
            });

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IAnswerRepository, AnswerRepository>();
            services.AddScoped<IExamRepository, ExamRepository>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IExamRepository, ExamRepository>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();

            return services;
        }
    }
}