using Microsoft.AspNetCore.Mvc.ModelBinding;
using FluentValidation.Results;


namespace book_app_api.Infrastructure.Extensions;

public static class ControllerExtensions
{
    public static void AddModelErrors(this ModelStateDictionary modelState, ValidationResult result)
    {
        foreach (var error in result.Errors) 
            modelState.AddModelError(error.PropertyName, error.ErrorMessage);
    }
}