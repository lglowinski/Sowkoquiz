using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sowkoquiz.Domain.QuizzDefinitionAggregate;

namespace Sowkoquiz.Infrastructure.Persistance.Configurations.QuizDefinitionConfiguration;

public class QuizDefinitionConfiguration : IEntityTypeConfiguration<QuizzDefinition>
{
    public void Configure(EntityTypeBuilder<QuizzDefinition> builder)
    {
        builder.Property(q => q.Id).ValueGeneratedOnAdd();

        builder.HasKey(q => q.Id);
        builder.HasIndex(q => q.Description);
        builder.HasIndex(q => q.Title);
        
        builder.HasMany(q => q.QuestionPool)
            .WithOne()
            .HasForeignKey("QuizzDefinitionId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}