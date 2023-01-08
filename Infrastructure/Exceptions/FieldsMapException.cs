using System.Diagnostics.CodeAnalysis;

namespace book_app_api.Infrastructure.Exceptions;


[ExcludeFromCodeCoverage]
public class FieldsMapException : Exception
{
    public FieldsMapException(Dictionary<string, string> fieldErrors)
    {
        FieldErrors = fieldErrors;
    }
    
    public Dictionary<string, string> FieldErrors { get; init;}
}