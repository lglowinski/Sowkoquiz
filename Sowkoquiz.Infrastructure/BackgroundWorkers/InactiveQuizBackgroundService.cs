using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sowkoquiz.Application.Common;
using Sowkoquiz.Domain.ActiveQuizEntity;
using Sowkoquiz.Domain.Common;

namespace Sowkoquiz.Infrastructure.BackgroundWorkers;

public class InactiveQuizBackgroundService(
    IServiceScopeFactory serviceScopeFactory,
    int retentionTime,
    ILogger<InactiveQuizBackgroundService> logger,
    IDateTimeProvider dateTimeProvider) : IHostedService
{
    private Task? _processQuizzesTask;
    private PeriodicTimer? _timer;
    private readonly CancellationTokenSource _cts = new();

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _processQuizzesTask = ProcessQuizzesAsync();
        return Task.CompletedTask;
    }

    private async Task ProcessQuizzesAsync()
    {
        logger.LogInformation("Starting inactive quiz background service.");

        _timer = new PeriodicTimer(TimeSpan.FromMinutes(retentionTime));

        while (await _timer.WaitForNextTickAsync(_cts.Token))
        {
            try
            {
                await ProcessQuizzes();
            }
            catch (Exception e)
            {
                logger.LogError(e, "Exception occurred while publishing domain events.");
            }
        }
    }

    private async Task ProcessQuizzes()
    {
        var activeQuizRepository = serviceScopeFactory.CreateScope().ServiceProvider
            .GetRequiredService<IActiveQuizRepository>();

        var inactiveQuizzes = await activeQuizRepository.GetInactiveQuizzesAsync(retentionTime, _cts.Token);

        if (inactiveQuizzes.Count == 0)
            return;
        
        logger.LogInformation("Starting to process {Count} inactive quizzes.", inactiveQuizzes.Count);

        foreach (var quiz in inactiveQuizzes)
        {
            quiz.Status = QuizStatus.Inactive;
            await activeQuizRepository.UpdateAsync(quiz, _cts.Token);
        }

        logger.LogInformation("Finished processing inactive quizzes. Next processing: {NextProcessingDate}",
            dateTimeProvider.UtcNow.AddMinutes(retentionTime));
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_processQuizzesTask is null)
            return;

        await _cts.CancelAsync();
        await _processQuizzesTask;

        _timer?.Dispose();
        _cts.Dispose();
    }
}