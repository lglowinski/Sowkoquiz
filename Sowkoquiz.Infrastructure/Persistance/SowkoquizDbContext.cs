using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Sowkoquiz.Domain.ActiveQuizEntity;
using Sowkoquiz.Domain.Common;
using Sowkoquiz.Domain.QuestionEntity;
using Sowkoquiz.Domain.QuizzDefinitionAggregate;
using Sowkoquiz.Infrastructure.DomainEvents;

namespace Sowkoquiz.Infrastructure.Persistance;

public class SowkoquizDbContext(DbContextOptions<SowkoquizDbContext> options, DomainEventsQueue queue) : DbContext(options)
{
    public DbSet<ActiveQuiz> ActiveQuizzes { get; init; }
    public DbSet<QuizzDefinition> QuizzDefinitions { get; init; }
    public DbSet<Question> Questions { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEvents = ChangeTracker
            .Entries<AggregateRoot>()
            .Select(entry => entry.Entity.PopDomainEvents())
            .SelectMany(@event => @event);

        var result = await base.SaveChangesAsync(cancellationToken);

        foreach (var domainEvent in domainEvents)
        {
            queue.Enqueue(domainEvent);
        }
        
        return result;
    }
}