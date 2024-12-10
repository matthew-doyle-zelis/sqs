namespace Api.Configuration;

public class AwsOptions
{
    public const string Section = "AWS";
    public required string Region { get; set; }
    public required string AccessKey { get; set; }
    public required string SecretKey { get; set; }
    public string? ServiceUrl { get; set; }  // For LocalStack
}