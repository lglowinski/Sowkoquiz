using System.Reflection;
using System.Text.Json;
using Sowkoquiz.Domain.QuizzDefinitionAggregate;

namespace Sowkoquiz.Migration;

public class SeedDataLoader(TextWriter textWriter)
{
    public async Task<IEnumerable<QuizzDefinition>> GetData(CancellationToken cancellationToken)
    {
        var assembly = Assembly.GetExecutingAssembly();
        const string resourceName = "Sowkoquiz.Migration.seed.data.json";

        await using var stream = assembly.GetManifestResourceStream(resourceName);

        if (stream is null)
        {
            await textWriter.WriteLineAsync($"Resource not found: {resourceName}");
            return Enumerable.Empty<QuizzDefinition>();
        }
        
        var result = await JsonSerializer.DeserializeAsync<List<QuizzDefinition>>(stream, cancellationToken: cancellationToken);

        if (result is not null) 
            return result;
        
        await textWriter.WriteLineAsync("Failed to parse json");
        return Enumerable.Empty<QuizzDefinition>();
    }
}