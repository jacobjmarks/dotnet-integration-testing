using Microsoft.AspNetCore.Mvc;

namespace Azenix.Examples.IntegrationTesting.Api.Controllers;

[ApiController]
[Route("[controller]/books")]
public class BookController : ControllerBase
{
    private readonly ILogger<BookController> _logger;
    public static IList<Book> _bookStore;

    public BookController(ILogger<BookController> logger)
    {
        _logger = logger;
        _bookStore = new List<Book>();
    }

    [HttpGet("")]
    public IEnumerable<Book> GetAllBooks()
    {
        _logger.LogInformation("Successfully returned all books");
        return _bookStore;
    }

    [HttpGet("{id}")]
    public List<Book> GetBook(string id)
    {
        _logger.LogInformation("Successfully returned a book with id: {Id}", id);
        return _bookStore.Where(item => item.Id == id).ToList();
    }

    [HttpPost("{id}")]
    public void CreateBook([FromBody] Book book, string id)
    {
        _logger.LogInformation("Successfully created a book with id: {Id}", id);
        var item = new Book
        {
            Author = book.Id,
            Title = book.Id,
            UtcDatePublished = DateTime.MinValue,
            UtcDateAdded = DateTime.UtcNow,
            Isbn = book.Id,
            Id = id
        };
        _bookStore.Add(item);
    }

    // [HttpPut(Name = "UpdateBook")]
    // public void UpdateBook()
    // {
    //     
    // }
    //
    // [HttpDelete(Name = "DeleteBook")]
    // public void DeleteBook()
    // {
    //     
    // }
}