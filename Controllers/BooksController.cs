using Microsoft.AspNetCore.Mvc;
using book_app_api.Models;
using book_app_api.Services;


namespace book_app_api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/books")]
public class BooksController : ControllerBase
{
    private readonly IBooksService _booksService;

    public BooksController(IBooksService booksService)
    {
        _booksService = booksService;
    }

    [HttpGet("list")]
    [ProducesResponseType(typeof(List<Book>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllBooksAsync()
    {
        List<Book> allBooks = await _booksService.GetAllBooksAsync();
        return Ok(allBooks);
    }

    /// <summary>
    /// For API Versioning test
    /// </summary>
    [HttpGet("list")]
    [ApiVersion("2.0")]
    public Task<IActionResult> GetAllBooksAsync2()
    {
        return Task.FromResult<IActionResult>(Ok("lol kek"));
    }

    [HttpGet("{isbn}")]
    [ProducesResponseType(typeof(Book), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetBookAsync(string isbn)
    {
        Book book = await _booksService.GetBookAsync(isbn);
        return Ok(book);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Book), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddBookAsync(Book book)
    {
        try
        {
            await _booksService.AddBookAsync(book);
            return CreatedAtAction(nameof(GetBookAsync), new { isbn = book.Isbn }, book);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{isbn}")]
    public async Task<IActionResult> DeleteBookAsync(string isbn)
    {
        // DeleteAsync is used to delete an item from DynamoDB
        await _booksService.DeleteBookAsync(isbn);
        return Ok();
    }

    [HttpPut("{isbn}")]
    public async Task<IActionResult> UpdateBookAsync(string isbn, Book book)
    {
        try
        {
            await _booksService.UpdateBookAsync(isbn, book);
            return Ok(book);
        }
        catch (Exception e)
        {
            return NotFound();
        }
    }

    [HttpPatch("{isbn}")]
    public async Task<IActionResult> ToggleFavoriteAsync(string isbn)
    {
        try
        {
            var bookInBase = await _booksService.ToggleFavoriteAsync(isbn);
            return Ok(new { IsFavorite = bookInBase.IsFavorite });
        }
        catch (Exception)
        {
            return NotFound();
        }
    }

    [HttpGet("search/{title}")]
    public async Task<IActionResult> Search(string title)
    {
        List<Book> response = await _booksService.GetBooksByTitleAsync(title);
        return Ok(response);
    }
}