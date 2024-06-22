using CabaVS.ExpenseTracker.Domain.Errors.Shared;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Domain.ValueObjects;

namespace CabaVS.ExpenseTracker.Domain.Errors;

public static class TransactionErrors
{
    public static Error TagIsNullOrWhitespace() =>
        StringErrors.IsNullOrWhiteSpace("Transaction", "Tags");
    public static Error TagTooLong(string actualValue) =>
        StringErrors.TooLong("Transaction", "Tags", TransactionTag.MaxLength, actualValue);
    public static Error TagDuplication(string tag) =>
        new(
            "Transaction.Tags",
            $"Tag '{tag}' already exist on Transaction. Duplicates are not allowed.");
    public static Error TagContainsComma(string tag) =>
        new(
            "Transaction.Tags",
            $"Tag '{tag}' should not contain ',' symbol.");

    public static Error AmountShouldBeGreaterThanZero() =>
        new(
            "Transaction.AmountShouldBeGreaterThanZero",
            "Amount should be greater than zero in Transaction.");
    
    public static Error NotFoundById(Guid id) => GeneralErrors.NotFoundById("Transaction", id);
}