using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sowkoquiz.Domain.ActiveQuizEntity;

namespace Sowkoquiz.Infrastructure.Persistance.Configurations.ActiveQuizConfiguration;

public class ActiveQuizConfiguration : IEntityTypeConfiguration<ActiveQuiz>
{
    public void Configure(EntityTypeBuilder<ActiveQuiz> builder)
    {
        builder.Property(q => q.Id).ValueGeneratedOnAdd();

        builder.HasKey(q => q.Id);

        builder.OwnsOne(quiz => quiz.Progress, progress =>
        {
            progress.Property(p => p.Answered).HasColumnName("Answered");
            progress.Property(p => p.Correct).HasColumnName("Correct");
            progress.Property(p => p.Max).HasColumnName("Max");
        });
    }
}