using System.Runtime.CompilerServices;
using System.Text.Json;
using Sowkoquiz.Domain.QuizzDefinitionAggregate;

namespace Sowkoquiz.DataUploader;

public class DataReader(string path, ILogger<DataReader> logger)
{
    private readonly DirectoryInfo _dirInfo = new(path);

    public async IAsyncEnumerable<QuizzDefinition> ReadQuizzesAsync(
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var files = _dirInfo.EnumerateFiles("*.json");

        foreach (var file in files)
        {
            await foreach (var quiz in ProcessFiles(file, cancellationToken))
            {
                yield return quiz;
            }
        }
    }

    public void Refresh()
    {
        _dirInfo.Refresh();
    }

    private async IAsyncEnumerable<QuizzDefinition> ProcessFiles(FileInfo file,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Processing file: {File}", file.FullName);

        await foreach (var quiz in ReadQuizAsync(file, cancellationToken))
        {
            yield return quiz;
        }


        logger.LogInformation("Finished processing file: {File}", file.FullName);
    }

    private async IAsyncEnumerable<QuizzDefinition> ReadQuizAsync(FileInfo file,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        List<QuizzDefinition>? quizzes;
        await using(var stream = file.OpenRead())
        {
            quizzes =
                await JsonSerializer.DeserializeAsync<List<QuizzDefinition>>(stream,
                    cancellationToken: cancellationToken);

            if (quizzes is null)
            {
                logger.LogWarning("Failed to parse json from {File}", file.FullName);
                yield break;
            }
        }
        
        DeleteFile(file);
        foreach (var quiz in quizzes)
            yield return quiz;
    }

    private void DeleteFile(FileSystemInfo file)
    {
        logger.LogInformation("Deleting file: {File} after processing", file.FullName);
        file.Delete();
    }
}