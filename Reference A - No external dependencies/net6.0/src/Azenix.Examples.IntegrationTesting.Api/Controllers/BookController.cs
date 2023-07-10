using Microsoft.AspNetCore.Mvc;

namespace Azenix.Examples.IntegrationTesting.Api.Controllers;

[ApiController]
[Route("[controller]/books")]
public class BookController : ControllerBase
{
    private readonly ILogger<BookController> _logger;
    private static Dictionary<string, Book> _bookStore = new();

    public BookController(ILogger<BookController> logger)
    {
        _logger = logger;
    }

    [HttpGet("")]
    public IEnumerable<Book> GetAllBooks()
    {
        _logger.LogInformation("Successfully returned all books");
        return _bookStore.Values.ToList();
    }

    [HttpGet("{id}")]
    public Book? GetBook(string id)
    {
        _logger.LogInformation("Successfully returned a book with id: {Id}", id);
        return _bookStore.TryGetValue(id, out var value) ? value : null;
    }

    [HttpPost("{id}")]
    public void CreateBook([FromBody] Book book, string id)
    {
        _logger.LogInformation("Successfully created a book with id: {Id}", id);
        var item = new Book
        {
            Author = book.Author,
            Title = book.Title,
            UtcDatePublished = DateTime.MinValue,
            UtcDateAdded = DateTime.UtcNow,
            Isbn = book.Id,
            Id = id
        };
        _bookStore.Add(id,item);
    }

    [HttpPut("{id}")]
    public void UpdateBook([FromBody] Book book, string id)
    {
        _logger.LogInformation("Successfully updated book with id: {Id}", id);
        var item = new Book
        {
            Author = book.Author,
            Title = book.Title,
            UtcDatePublished = DateTime.MinValue,
            UtcDateAdded = DateTime.UtcNow,
            Isbn = book.Id,
            Id = id
        };

        _bookStore[id] = item;
    }

    [HttpDelete(Name = "DeleteBook")]
    public void DeleteBook(string id)
    {
        _bookStore.Remove(id);
    }
}