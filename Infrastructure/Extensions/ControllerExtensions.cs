using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;


namespace book_app_api.Infrastructure.Extensions;

public static class ControllerExtensions
{
    public static void AddModelErrors(this ModelStateDictionary modelState, ValidationResult result)
    {
        foreach (var error in result.Errors) 
            modelState.AddModelError(error.PropertyName, error.ErrorMessage);
    }
}