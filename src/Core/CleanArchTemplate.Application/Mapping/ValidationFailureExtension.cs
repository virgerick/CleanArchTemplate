using CleanArchTemplate.Shared.Results;
using FluentValidation.Results;

namespace CleanArchTemplate.Application.Mapping;

public static class ValidationFailureExtension
{
    public static ValidationError ToValidationError(this IEnumerable<ValidationFailure> failures)
    {
        return ValidationError.Create(failures.Map());
    }

    public static Dictionary<string,string[]> Map(this IEnumerable<ValidationFailure> failures)
    =>failures
        .GroupBy(failure => failure.PropertyName)
        .ToDictionary(
            group => group.Key,
            group => group.Select(failure => failure.ErrorMessage).ToArray()
        );

   
}