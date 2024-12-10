namespace Api.Configuration;

public class QueueOptions
{
    public const string Section = "Queue";
    public required string ActivityQueueName { get; set; }
    public int ProcessingDelaySeconds { get; set; } = 15;
    public int MessageRetentionPeriod { get; set; } = 1209600; // 14 days
    public int VisibilityTimeout { get; set; } = 30;
}