using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Primitives;
using CabaVS.ExpenseTracker.Domain.Shared;

namespace CabaVS.ExpenseTracker.Domain.Entities;

public sealed class ExpenseTransaction : Entity
{
    public DateOnly Date { get; }
    
    public Balance Source { get; }
    public decimal AmountInSourceCurrency { get; }
    
    public ExpenseCategory Destination { get; }
    public decimal AmountInDestinationCurrency { get; }
    
    private ExpenseTransaction(
        Guid id, 
        DateOnly date, 
        Balance source,
        decimal amountInSourceCurrency, 
        ExpenseCategory destination,
        decimal amountInDestinationCurrency) : base(id)
    {
        Date = date;

        Source = source;
        AmountInSourceCurrency = amountInSourceCurrency;
        
        Destination = destination;
        AmountInDestinationCurrency = amountInDestinationCurrency;
    }

    public static Result<ExpenseTransaction> Create(
        Guid id, 
        DateOnly dateInUtc, 
        Balance source,
        ExpenseCategory destination,
        decimal amount)
    {
        return source.Currency == destination.Currency 
            ? Create(id, dateInUtc, source, destination, amount, amount)
            : TransactionErrors.AmountInDestinationCurrencyNotProvided();
    }

    public static Result<ExpenseTransaction> Create(
        Guid id,
        DateOnly dateInUtc,
        Balance source,
        ExpenseCategory destination,
        decimal amountInSourceCurrency,
        decimal amountInDestinationCurrency)
    {
        if (amountInSourceCurrency <= 0 || amountInDestinationCurrency <= 0)
            return TransactionErrors.AmountShouldBeGreaterThanZero();

        source.Amount -= amountInSourceCurrency;
        
        return new ExpenseTransaction(id, dateInUtc, source, amountInSourceCurrency, destination, amountInDestinationCurrency);
    }
}