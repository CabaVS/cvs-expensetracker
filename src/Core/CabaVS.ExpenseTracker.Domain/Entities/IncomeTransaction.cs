using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Primitives;
using CabaVS.ExpenseTracker.Domain.Shared;

namespace CabaVS.ExpenseTracker.Domain.Entities;

public sealed class IncomeTransaction : Entity
{
    public DateOnly Date { get; }
    
    public IncomeCategory Source { get; }
    public decimal AmountInSourceCurrency { get; }
    
    public Balance Destination { get; }
    public decimal AmountInDestinationCurrency { get; }
    
    private IncomeTransaction(
        Guid id, 
        DateOnly date, 
        IncomeCategory source,
        decimal amountInSourceCurrency, 
        Balance destination,
        decimal amountInDestinationCurrency) : base(id)
    {
        Date = date;

        Source = source;
        AmountInSourceCurrency = amountInSourceCurrency;
        
        Destination = destination;
        AmountInDestinationCurrency = amountInDestinationCurrency;
    }

    public static Result<IncomeTransaction> Create(
        Guid id, 
        DateOnly dateInUtc, 
        IncomeCategory source,
        Balance destination,
        decimal amount)
    {
        return source.Currency == destination.Currency 
            ? Create(id, dateInUtc, source, destination, amount, amount)
            : TransactionErrors.AmountInDestinationCurrencyNotProvided();
    }

    public static Result<IncomeTransaction> Create(
        Guid id,
        DateOnly dateInUtc,
        IncomeCategory source,
        Balance destination,
        decimal amountInSourceCurrency,
        decimal amountInDestinationCurrency)
    {
        if (amountInSourceCurrency <= 0 || amountInDestinationCurrency <= 0)
            return TransactionErrors.AmountShouldBeGreaterThanZero();

        destination.Amount += amountInDestinationCurrency;
        
        return new IncomeTransaction(id, dateInUtc, source, amountInSourceCurrency, destination, amountInDestinationCurrency);
    }
}