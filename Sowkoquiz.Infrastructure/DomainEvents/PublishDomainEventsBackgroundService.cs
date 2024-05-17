using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sowkoquiz.Application;

namespace Sowkoquiz.Infrastructure.DomainEvents;

public class PublishDomainEventsBackgroundService(
    IDomainEventsQueue queue,
    IServiceScopeFactory serviceScopeFactory,
    ILogger
        <PublishDomainEventsBackgroundService> logger)
    : IHostedService
{
    private Task? _processEventsTask;
    private PeriodicTimer? _timer;
    private readonly CancellationTokenSource _cts = new();

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _processEventsTask = ProcessEventsAsync();
        return Task.CompletedTask;
    }

    private async Task ProcessEventsAsync()
    {
        logger.LogInformation("Starting domain event publisher background service.");
        
        _timer = new PeriodicTimer(TimeSpan.FromSeconds(5));

        while (await _timer.WaitForNextTickAsync(_cts.Token))
        {
            try
            {
                await PublishDomainEvents();
            }
            catch (Exception e)
            {
                logger.LogError(e, "Exception occurred while publishing domain events.");
            }
        }
    }

    private async Task PublishDomainEvents()
    {
        var publisher = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IPublisher>();

        while (queue.TryDequeue(out var @event))
        {
            await publisher.Publish(@event);
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_processEventsTask is null)
            return;
        
        await _cts.CancelAsync();
        await _processEventsTask;
        
        _timer?.Dispose();
        _cts.Dispose();
    }
}