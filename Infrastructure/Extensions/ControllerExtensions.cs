using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace book_app_api.Infrastructure.Extensions;

public static class ControllerExtensions
{
    public static Dictionary<string, List<string>> AddModelError(this ValidationResult result)
    {
        var dict = new Dictionary<string, List<string>>();
        foreach (var error in result.Errors) 
        {
            if (dict.TryGetValue(error.PropertyName, out List<string>? errors))
                errors.Add(error.ErrorMessage);
            dict.Add(error.PropertyName, new List<string> {error.ErrorMessage});
        }

        return dict;
    }
}