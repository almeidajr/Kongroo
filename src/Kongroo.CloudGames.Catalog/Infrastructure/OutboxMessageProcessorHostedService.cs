using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Kongroo.CloudGames.Catalog.Infrastructure;

public sealed class OutboxMessageProcessorHostedService(
    IServiceScopeFactory serviceScopeFactory,
    ILogger<OutboxMessageProcessorHostedService> logger,
    TimeProvider timeProvider,
    IOptions<OutboxProcessingOptions> options
) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Catalog outbox processor hosted service started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await using var scope = serviceScopeFactory.CreateAsyncScope();
                var processor = scope.ServiceProvider.GetRequiredService<OutboxMessageProcessor>();

                await processor.ProcessPendingMessagesAsync(stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Unexpected error while processing catalog outbox messages.");
            }

            await Task.Delay(options.Value.PollingInterval, timeProvider, stoppingToken);
        }
    }
}
