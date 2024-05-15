using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sowkoquiz.Domain.QuestionEntity;

namespace Sowkoquiz.Infrastructure.Persistance.Configurations.QuestionConfiguration;

public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.Property(q => q.Id).ValueGeneratedOnAdd();

        builder.HasKey(q => q.Id);
        
        builder.OwnsMany(question => question.Answers, answer =>
        {
            answer.WithOwner().HasForeignKey("QuestionId");
            answer.Property(a => a.Text).HasColumnName("AnswerText");
            answer.Property(a => a.IsCorrect).HasColumnName("IsCorrect");
            answer.Property(a => a.Letter).HasColumnName("Letter");
            answer.HasKey("QuestionId", "Letter");
        });
    }
}