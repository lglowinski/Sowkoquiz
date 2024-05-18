using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sowkoquiz.Application;
using Sowkoquiz.Application.Common;
using Sowkoquiz.Domain.Common;
using Sowkoquiz.Infrastructure.BackgroundWorkers;
using Sowkoquiz.Infrastructure.DomainEvents;
using Sowkoquiz.Infrastructure.Persistance;
using Sowkoquiz.Infrastructure.Persistance.Repositories;

namespace Sowkoquiz.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, int retentionTime = 15)
    {
        services.AddDatabase();

        services.AddSingleton<IDomainEventsQueue, DomainEventsQueue>();

        services
            .AddBackgroundService(retentionTime)
            .AddDateTimeProvider();

        return services;
    }

    private static IServiceCollection AddBackgroundService(this IServiceCollection services, int retentionTime)
    {
        services.AddHostedService<PublishDomainEventsBackgroundService>();
        services.AddHostedService<InactiveQuizBackgroundService>(sp =>
            new InactiveQuizBackgroundService(sp.GetRequiredService<IServiceScopeFactory>(), retentionTime,
                sp.GetRequiredService<ILogger<InactiveQuizBackgroundService>>(),
                sp.GetRequiredService<IDateTimeProvider>()));
        return services;
    }

    private static IServiceCollection AddDateTimeProvider(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();
        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services.AddDbContext<SowkoquizDbContext>(options
            => options.UseSqlite("Data Source = Sowkoquiz.db"));

        services.AddScoped<IQuestionRepository, QuestionsRepository>();
        services.AddScoped<IActiveQuizRepository, ActiveQuizRepository>();
        services.AddScoped<IQuizDefinitionRepository, QuizDefinitionRepository>();

        return services;
    }
}