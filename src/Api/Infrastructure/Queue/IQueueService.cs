using Api.Models;

namespace Api.Infrastructure.Queue;

public interface IQueueService
{
    Task<string> EnqueueActivityAsync(Activity activity);
    Task<string> ReceiveMessageAsync();
    Task DeleteMessageAsync(string message);
}