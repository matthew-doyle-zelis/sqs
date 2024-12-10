namespace Api.Models;

public class QueueStatus
{
    public required string TrackingId { get; set; }
    public required ProcessingStatus Status { get; set; }    // Could be an enum
    public required DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public string? Error { get; set; }
}

// If using an enum:
public enum ProcessingStatus
{
    Queued,
    Processing,
    Completed,
    Failed
}