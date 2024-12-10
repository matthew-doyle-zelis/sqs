namespace Api.Models;

public class Activity
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
}