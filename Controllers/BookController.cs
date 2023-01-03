using Microsoft.AspNetCore.Mvc;

using book_app_api.Models;
using book_app_api.Services;


namespace book_app_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController : ControllerBase
{
    private readonly IBookService _bookService;

    public BookController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> GetAllBooksAsync()
    {
        List<Book> allBooks = await _bookService.GetAllBooksAsync();
        return Ok(allBooks);
    }

    [HttpGet]
    [Route("{isbn}")]
    public async Task<IActionResult> GetBookAsync(string isbn)
    {
        Book book = await _bookService.GetBookAsync(isbn);
        return base.Ok(book);
    }

    [HttpPost]
    public async Task<IActionResult> AddBookAsync(Book book)
    {
        await _bookService.AddBookAsync(book);
        return base.Ok(book);
    }

    [HttpDelete]
    [Route("{isbn}")]
    public async Task<IActionResult> DeleteBookAsync(string isbn)
    {
        // DeleteAsync is used to delete an item from DynamoDB
        await _bookService.DeleteBookAsync(isbn);
        return base.Ok();
    }

    [HttpPut]
    [Route("{isbn}")]
    public async Task<IActionResult> UpdateBookAsync(string isbn, Book book)
    {
        try
        {
            await _bookService.UpdateBookAsync(isbn, book);
            return Ok(book);
        }
        catch (Exception e)
        {
            return NotFound();
        }
    }
    
    [HttpPatch]
    [Route("{isbn}")]
    public async Task<IActionResult> ToggleFavoriteAsync(string isbn)
    {
        try
        {
            var bookInBase = await _bookService.ToggleFavoriteAsync(isbn);
            return Ok(new {IsFavorite = bookInBase.IsFavorite});
        }
        catch (Exception )
        {
            return NotFound();
        }
    }

    [HttpGet]
    [Route("search/{title}")]
    public async Task<IActionResult> Search(string title)
    {
        List<Book> response = await _bookService.GetBooksByTitleAsync(title);
        return Ok(response);
    }
}