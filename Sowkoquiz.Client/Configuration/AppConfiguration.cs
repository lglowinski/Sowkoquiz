using Sowkoquiz.Common.Options;

namespace Sowkoquiz.Configuration;

public class AppConfiguration : IAppConfiguration
{
    public static string SectionName => "AppConfiguration";
    
    public Uri? GrpcBaseUrl { get; set; } = default;
    
}