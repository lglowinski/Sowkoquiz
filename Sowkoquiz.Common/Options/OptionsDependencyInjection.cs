using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Sowkoquiz.Common.Options.Exceptions;

namespace Sowkoquiz.Common.Options;

public static class OptionsDependencyInjection
{
    public static IOptions<TOptions> RegisterAndGetOptions<TOptions>(this IServiceCollection services) where TOptions : class, IAppConfiguration
    {
        var options = services
            .RegisterOptions<TOptions>()
            .BuildServiceProvider()
            .GetService<IOptions<TOptions>>();

        if (options is null)
            throw new OptionsConfigurationException<TOptions>();
        
        return options;
    }
    
    public static IServiceCollection RegisterOptions<TOptions>(this IServiceCollection services) where TOptions : class, IAppConfiguration
    {
        var property = typeof(TOptions).GetProperty(nameof(IAppConfiguration.SectionName));
        
        if(property is null)
            throw new ArgumentException("The property SectionName is not defined in the IAppConfiguration interface");

        var sectionName = property.GetValue(null) as string;
        
        services.AddOptions<IAppConfiguration>(sectionName).Configure(_ => {});

        return services;
    }
}