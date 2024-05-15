using Sowkoquiz.Common.Options;

namespace Sowkoquiz.Grpc.Configuration;

public class AppConfiguration : IAppConfiguration
{
    public static string SectionName => "AppConfiguration";

    public int QuizzRetentionTimeInMinutes { get; init; } = 15;
}