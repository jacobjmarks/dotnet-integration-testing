namespace Azenix.Examples.IntegrationTesting.Api.Controllers;

public class Book
{
    public string? Author { get; set; }
    public string? Title { get; set; }
    public string? Isbn { get; set; }
    public string? Id { get; set; }
    public DateTime UtcDatePublished { get; set; }
    public DateTime UtcDateAdded { get; set; }
}