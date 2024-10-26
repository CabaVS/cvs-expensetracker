using CabaVS.ExpenseTracker.Domain.Shared;

namespace CabaVS.ExpenseTracker.Domain.Errors.Shared;

public static class StringErrors
{
    public static Error IsNullOrWhiteSpace(string entityName, string propertyName) =>
        new($"{entityName}.{propertyName}IsNullOrWhiteSpace",
            $"{entityName} {propertyName} should not be NULL or whitespace.");
    
    public static Error TooLong(string entityName, string propertyName, int maxLength, string actualValue) =>
        new($"{entityName}.{propertyName}TooLong",
            $"{entityName} {propertyName} should be {maxLength} characters or shorter. Actual length was {actualValue.Length} characters.");
    
    public static Error TooShort(string entityName, string propertyName, int minLength, string actualValue) =>
        new($"{entityName}.{propertyName}TooShort",
            $"{entityName} {propertyName} should be {minLength} characters or longer. Actual length was {actualValue.Length} characters.");
}