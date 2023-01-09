using System.Diagnostics.CodeAnalysis;
using FluentValidation.Results;


namespace book_app_api.Infrastructure.Exceptions;

[ExcludeFromCodeCoverage]
public class ModelValidationException : Exception
{
    public ModelValidationException(ValidationResult result)
    {
        ValidationResult = result;
    }

    public ValidationResult ValidationResult { get; }
}