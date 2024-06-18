using CabaVS.ExpenseTracker.Domain.Errors.Shared;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Domain.ValueObjects;

namespace CabaVS.ExpenseTracker.Domain.Errors;

public static class TransactionErrors
{
    public static Error TagIsNullOrWhitespace() =>
        StringErrors.IsNullOrWhiteSpace("Transaction", "Tag");
    public static Error TagTooLong(string actualValue) =>
        StringErrors.TooLong("Transaction", "Tag", TransactionTag.MaxLength, actualValue);
    public static Error TagDuplication(string tag) =>
        new(
            "Transaction.Tag",
            $"Tag '{tag}' already exist on Transaction. Duplicates are not allowed.");
    public static Error TagContainsComma(string tag) =>
        new(
            "Transaction.Tag",
            $"Tag '{tag}' should not contain ',' symbol.");
    
    public static Error AmountInDestinationCurrencyNotProvided() =>
        new(
            "Transaction.AmountInDestinationCurrencyNotProvided",
            "Amount in Destination Currency is required when creating Transaction between objects with different Currencies.");
    
    public static Error AmountShouldBeGreaterThanZero() =>
        new(
            "Transaction.AmountShouldBeGreaterThanZero",
            "Amount should be greater than zero in Transaction.");
    
    public static Error NotFoundById(Guid id) => GeneralErrors.NotFoundById("Transaction", id);
}