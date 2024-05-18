using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sowkoquiz.Domain.ActiveQuizEntity;

namespace Sowkoquiz.Infrastructure.Persistance.Configurations.ActiveQuizConfiguration;

public class ActiveQuizConfiguration : IEntityTypeConfiguration<ActiveQuiz>
{
    public void Configure(EntityTypeBuilder<ActiveQuiz> builder)
    {
        builder.Property(q => q.Id).ValueGeneratedOnAdd();
        builder.Property(q => q.EndTime).HasConversion(new DateTimeOffsetToBinaryConverter());
        
        builder.HasKey(q => q.Id);
        builder.HasIndex(q => q.EndTime);

        builder.HasQueryFilter(q => q.Status != QuizStatus.Deleted);
        
        builder.OwnsOne(quiz => quiz.Progress, progress =>
        {
            progress.Property(p => p.Answered).HasColumnName("Answered");
            progress.Property(p => p.Correct).HasColumnName("Correct");
            progress.Property(p => p.Max).HasColumnName("Max");
        });

        builder.OwnsMany(quiz => quiz.AnsweredQuestions, answered =>
        {
            answered.WithOwner().HasForeignKey("ActiveQuizId");
            answered.Property(a => a.Letter).HasColumnName("Letter");
            answered.HasKey("Id", "ActiveQuizId", "Letter");
        });
    }
}