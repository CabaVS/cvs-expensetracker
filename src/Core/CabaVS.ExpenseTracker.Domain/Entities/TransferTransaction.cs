using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Primitives;
using CabaVS.ExpenseTracker.Domain.Shared;

namespace CabaVS.ExpenseTracker.Domain.Entities;

public sealed class TransferTransaction : Entity
{
    public DateOnly Date { get; }
    
    public Balance Source { get; }
    public decimal AmountInSourceCurrency { get; }
    
    public Balance Destination { get; }
    public decimal AmountInDestinationCurrency { get; }
    
    private TransferTransaction(
        Guid id, 
        DateOnly date, 
        Balance source,
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

    public static Result<TransferTransaction> Create(
        Guid id, 
        DateOnly dateInUtc, 
        Balance source,
        Balance destination,
        decimal amount)
    {
        return source.Currency == destination.Currency 
            ? Create(id, dateInUtc, source, destination, amount, amount)
            : TransactionErrors.AmountInDestinationCurrencyNotProvided();
    }

    public static Result<TransferTransaction> Create(
        Guid id,
        DateOnly dateInUtc,
        Balance source,
        Balance destination,
        decimal amountInSourceCurrency,
        decimal amountInDestinationCurrency)
    {
        if (amountInSourceCurrency <= 0 || amountInDestinationCurrency <= 0)
            return TransactionErrors.AmountShouldBeGreaterThanZero();

        source.Amount -= amountInSourceCurrency;
        destination.Amount += amountInDestinationCurrency;
        
        return new TransferTransaction(id, dateInUtc, source, amountInSourceCurrency, destination, amountInDestinationCurrency);
    }
}