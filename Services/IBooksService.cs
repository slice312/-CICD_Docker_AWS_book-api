using book_app_api.Models;


namespace book_app_api.Services;

public interface IBooksService
{
    Task<List<Book>> GetAllBooksAsync();

    Task<Book> GetBookAsync(string isbn);

    Task AddBookAsync(Book book);

    Task<Book> UpdateBookAsync(string isbn, Book book);

    Task DeleteBookAsync(string isbn);

    Task<List<Book>> GetBooksByTitleAsync(string title);

    Task<Book> ToggleFavoriteAsync(string isbn);
}