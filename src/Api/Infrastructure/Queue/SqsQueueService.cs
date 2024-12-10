using Api.Infrastructure.Queue;
using Api.Models;
using Amazon.SQS;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Api.Configuration;
using Amazon.SQS.Model;

namespace Api.Infrastructure.Queue;

public class SqsQueueService(
    IAmazonSQS sqsClient,
    IOptions<QueueOptions> queueOptions) : IQueueService
{
    private readonly IAmazonSQS _sqsClient = sqsClient;
    private readonly IOptions<QueueOptions> _queueOptions = queueOptions;
    private string? _queueUrl;

    private async Task EnsureQueueExists()
    {
        if (string.IsNullOrEmpty(_queueUrl))
        {
            var queueUrlResponse = await _sqsClient.GetQueueUrlAsync(_queueOptions.Value.ActivityQueueName);
            _queueUrl = queueUrlResponse.QueueUrl;
        }
    }

    public async Task<string> EnqueueActivityAsync(Activity activity)
    {
        await EnsureQueueExists();

        var request = new SendMessageRequest
        {
            QueueUrl = _queueUrl,
            MessageBody = JsonSerializer.Serialize(activity),
            DelaySeconds = _queueOptions.Value.ProcessingDelaySeconds
        };

        var response = await _sqsClient.SendMessageAsync(request);
        return response.MessageId;
    }

    public async Task<string> ReceiveMessageAsync()
    {
        await EnsureQueueExists();
        var request = new ReceiveMessageRequest { QueueUrl = _queueUrl };
        var response = await _sqsClient.ReceiveMessageAsync(request);
        return response.Messages[0].Body;
    }

    public async Task DeleteMessageAsync(string message)
    {
        await EnsureQueueExists();
        var request = new DeleteMessageRequest { QueueUrl = _queueUrl, ReceiptHandle = message };
        await _sqsClient.DeleteMessageAsync(request);
    }
}