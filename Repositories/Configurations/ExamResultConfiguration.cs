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
            builder.Property(er => er.Score).HasColumnType("decimal(10,2)");
            builder.Property(er => er.StartDate).IsRequired();
            builder.Property(er => er.CompletionDate);
            builder.Property(er => er.Duration);
            builder.Property(er => er.TotalQuestions).IsRequired();
            builder.Property(er => er.CorrectAnswers);
            builder.Property(er => er.IncorrectAnswers);

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
