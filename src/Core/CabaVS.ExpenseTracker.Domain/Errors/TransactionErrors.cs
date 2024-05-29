using CabaVS.ExpenseTracker.Domain.Shared;

namespace CabaVS.ExpenseTracker.Domain.Errors;

public static class TransactionErrors
{
    public static Error AmountInDestinationCurrencyNotProvided() =>
        new(
            "Transaction.AmountInDestinationCurrencyNotProvided",
            "Amount in Destination Currency is required when creating Transaction between objects with different Currencies.");
    
    public static Error AmountShouldBeGreaterThanZero() =>
        new(
            "Transaction.AmountShouldBeGreaterThanZero",
            "Amount should be greater than zero in Transaction.");
}