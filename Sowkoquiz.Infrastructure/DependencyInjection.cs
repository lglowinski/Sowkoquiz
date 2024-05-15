using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sowkoquiz.Application.Common;
using Sowkoquiz.Domain.Common;
using Sowkoquiz.Infrastructure.DomainEvents;
using Sowkoquiz.Infrastructure.Persistance;
using Sowkoquiz.Infrastructure.Persistance.Repositories;

namespace Sowkoquiz.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDatabase();

        services.AddSingleton<DomainEventsQueue>();

        services
            .AddBackgroundService()
            .AddDateTimeProvider();

        return services;
    }

    private static IServiceCollection AddBackgroundService(this IServiceCollection services)
    {
        services.AddHostedService<PublishDomainEventsBackgroundService>();
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