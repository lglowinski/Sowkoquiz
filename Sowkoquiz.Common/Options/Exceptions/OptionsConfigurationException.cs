namespace Sowkoquiz.Common.Options.Exceptions;

public class OptionsConfigurationException<TOptions>() : Exception(string.Format(DefaultMessage, typeof(TOptions).Name))
    where TOptions : IAppConfiguration
{
    private const string DefaultMessage = "Options retrival failed for {0}";
}