using ExamApp.Repositories.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExamApp.Repositories.Configurations
{
    public class ExamResultConfiguration : IEntityTypeConfiguration<ExamResult>
    {
        public void Configure(EntityTypeBuilder<ExamResult> builder)
        {
            builder.HasKey(er => er.ResultId);
            builder.Property(er => er.Score).IsRequired();
            builder.Property(er => er.CompletionDate).IsRequired();
            builder.Property(er => er.TotalQuestions).IsRequired();
            builder.Property(er => er.CorrectAnswers).IsRequired();
            builder.Property(er => er.IncorrectAnswers).IsRequired();

            builder.HasOne(er => er.User)
                .WithMany()
                .HasForeignKey(er => er.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(er => er.Exam)
                .WithMany()
                .HasForeignKey(er => er.ExamId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
