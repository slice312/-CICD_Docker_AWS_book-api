using FluentValidation;
using book_app_api.Models;


namespace book_app_api.Services;

public class BookValidator : AbstractValidator<Book>
{
    public BookValidator(IServiceProvider services)
    {
        RuleSet("Add", () =>
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
                    var booksService = services.GetRequiredService<IBooksService>();
                    Book existingBook = await booksService.GetBookAsync(isbn);
                    return existingBook is null;
                })
                .WithMessage("Book with isbn '{PropertyValue}' already exists")
                .OverridePropertyName("isbn");
        });
        
        
        RuleSet("Update", () =>
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
                .NotEmpty()
                .WithMessage("ISBN required")
                .OverridePropertyName("isbn");
        });
    }
}