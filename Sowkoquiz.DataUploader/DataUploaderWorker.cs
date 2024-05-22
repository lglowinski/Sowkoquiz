using Sowkoquiz.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Sowkoquiz.Domain.Common;

namespace Sowkoquiz.DataUploader;

public class DataUploaderWorker(
    ILogger<DataUploaderWorker> logger,
    IServiceScopeFactory serviceScopeFactory,
    DataReader reader,
    IDateTimeProvider dateTimeProvider)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Starting data uploader worker.");

        while (!stoppingToken.IsCancellationRequested)
        {
            await using var dbContext =
                serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<SowkoquizDbContext>();

            var strategy = dbContext.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                var quizzes = reader.ReadQuizzesAsync(stoppingToken).ToBlockingEnumerable();

                await using var transaction = await dbContext.Database.BeginTransactionAsync(stoppingToken);
                await dbContext.QuizzDefinitions.AddRangeAsync(quizzes, stoppingToken);
                await dbContext.SaveChangesAsync(stoppingToken);
                await transaction.CommitAsync(stoppingToken);
            });

            reader.Refresh();
            var waitTime = TimeSpan.FromMinutes(10);
            logger.LogInformation("Data uploaded - next check at {Time}", dateTimeProvider.UtcNow + waitTime);
            await Task.Delay(waitTime, stoppingToken);
        }
    }
}