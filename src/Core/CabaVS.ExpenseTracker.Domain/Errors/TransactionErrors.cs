using CabaVS.ExpenseTracker.Domain.Errors.Shared;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Domain.ValueObjects;

namespace CabaVS.ExpenseTracker.Domain.Errors;

public static class TransactionErrors
{
    public static Error NotFoundById<T>(Guid id) =>
        CommonErrors.NotFoundById(typeof(T).Name, id);
    
    public static Error SourceAndDestinationAreSame<T>() =>
        new(
            $"{typeof(T).Name}.{nameof(SourceAndDestinationAreSame)}",
            "The source and destination are the same Entity.");
    
    public static Error AmountShouldBePositive<T>(decimal amount, string? paramName = null)
    {
        var code = $"{typeof(T).Name}.{nameof(AmountShouldBePositive)}";
        var description = $"All amount values must be positive, but provided '{amount}' value.";

        if (paramName is not null)
        {
            description += $" Parameter name: '{paramName}'.";
        }
        
        return new Error(code, description);
    }
    
    public static Error TransactionTagIsNullOrWhitespace() =>
        StringErrors.IsNullOrWhiteSpace(nameof(TransactionTag), "Transaction Tag value");
    
    public static Error TransactionTagIsTooLong(string actualValue) =>
        StringErrors.TooLong(nameof(TransactionTag), "Transaction Tag value", TransactionTag.MaxLength, actualValue);
    
    public static Error TransactionTagContainsComma() =>
        new(
            $"{nameof(TransactionTag)}.{nameof(TransactionTagContainsComma)}",
            "Transaction Tag should not contain commas.");
}
