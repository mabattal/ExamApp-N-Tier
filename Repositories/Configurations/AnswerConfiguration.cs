using ExamApp.Repositories.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExamApp.Repositories.Configurations
{
    public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
    {
        public void Configure(EntityTypeBuilder<Answer> builder)
        {
            builder.HasKey(x => x.AnswerId);
            builder.Property(x => x.SelectedAnswer).IsRequired();
            builder.HasOne(x => x.User).WithMany(x => x.Answers).HasForeignKey(x => x.UserId);

        }
    }
}
