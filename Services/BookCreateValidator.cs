using book_app_api.Models;
using FluentValidation;

namespace book_app_api.Services;

public class BookCreateValidator : AbstractValidator<Book>
{
    public BookCreateValidator(IBooksService booksService)
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title required")
            .OverridePropertyName("title");
        
        RuleFor(x => x.Pages)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Pages should be positive")
            .OverridePropertyName("pages");

        RuleFor(x => x.Isbn)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("ISBN required")
            .MustAsync(async (isbn, cancellation) =>
            {
                Book existingBook = await booksService.GetBookAsync(isbn);
                return existingBook is null;
            })
            .WithMessage("Book with isbn '{PropertyValue}' already exists")
            .OverridePropertyName("isbn");
    }
}