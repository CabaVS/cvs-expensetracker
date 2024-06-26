using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Primitives;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Domain.ValueObjects;

namespace CabaVS.ExpenseTracker.Domain.Entities;

public sealed class TransferTransaction : Entity
{
    public DateOnly Date { get; }
    
    public Balance Source { get; }
    public decimal AmountInSourceCurrency { get; }
    
    public Balance Destination { get; }
    public decimal AmountInDestinationCurrency { get; }
    
    public List<TransactionTag> Tags { get; }
    
    private TransferTransaction(
        Guid id, 
        DateOnly date, 
        Balance source,
        decimal amountInSourceCurrency, 
        Balance destination,
        decimal amountInDestinationCurrency,
        List<TransactionTag> tags) : base(id)
    {
        Date = date;

        Source = source;
        AmountInSourceCurrency = amountInSourceCurrency;
        
        Destination = destination;
        AmountInDestinationCurrency = amountInDestinationCurrency;

        Tags = tags;
    }

    public static Result<TransferTransaction> Create(
        Guid id,
        DateOnly dateInUtc,
        Balance source,
        Balance destination,
        decimal amountInSourceCurrency,
        decimal amountInDestinationCurrency,
        IEnumerable<string>? tags,
        bool recalculateBalances)
    {
        if (amountInSourceCurrency <= 0 || amountInDestinationCurrency <= 0)
            return TransactionErrors.AmountShouldBeGreaterThanZero();

        var tagsResult = TransactionTag.CreateMultiple(tags);
        if (tagsResult.IsFailure) return tagsResult.Error;
        
        var transaction = new TransferTransaction(
            id, dateInUtc, 
            source, amountInSourceCurrency, 
            destination, amountInDestinationCurrency,
            tagsResult.Value);

        if (recalculateBalances)
        {
            transaction.Source.ApplyTransaction(transaction);
            transaction.Destination.ApplyTransaction(transaction);
        }

        return transaction;
    }
}