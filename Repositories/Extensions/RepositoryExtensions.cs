using ExamApp.Repositories.Answers;
using ExamApp.Repositories.Database;
using ExamApp.Repositories.ExamResults;
using ExamApp.Repositories.Exams;
using ExamApp.Repositories.Interceptors;
using ExamApp.Repositories.Questions;
using ExamApp.Repositories.Users;
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

                options.AddInterceptors(new AuditDbContextInterceptor());
            });

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IAnswerRepository, AnswerRepository>();
            services.AddScoped<IExamRepository, ExamRepository>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IExamResultRepository, ExamResultRepository>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // **Veritabanı Seed İşlemi**
            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                AppDbContextSeed.SeedAsync(dbContext).Wait();
            }

            return services;
        }
    }
}