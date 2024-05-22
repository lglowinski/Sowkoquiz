using System.Reflection;

namespace Sowkoquiz.Providers;

public class VersionProvider(Assembly assembly)
{
    public string Version { get; } = assembly.GetName().Version?.ToString() ?? "Unknown";
}