using Api.Configuration;
using Api.Infrastructure.Data;
using Amazon.SQS;
using Amazon.SQS.Model;
using System.Text.Json;
using Api.Models;
using Api.Infrastructure.Queue;
public class QueueProcessorService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public QueueProcessorService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ActivityContext>();
                var queueService = scope.ServiceProvider.GetRequiredService<IQueueService>();


                var message = await queueService.ReceiveMessageAsync();
                if (message != null)
                {
                    var activity = JsonSerializer.Deserialize<Activity>(message);
                    if (activity != null)
                    {
                        await dbContext.Activities.AddAsync(activity, stoppingToken);
                        await dbContext.SaveChangesAsync(stoppingToken);

                        await queueService.DeleteMessageAsync(message);
                    }
                }
            }

            await Task.Delay(1000, stoppingToken);
        }
    }
}