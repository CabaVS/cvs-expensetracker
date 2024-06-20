using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Primitives;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Domain.ValueObjects;

namespace CabaVS.ExpenseTracker.Domain.Entities;

public sealed class ExpenseTransaction : Entity
{
    public DateOnly Date { get; }
    
    public Balance Source { get; }
    public decimal AmountInSourceCurrency { get; }
    
    public ExpenseCategory Destination { get; }
    public decimal AmountInDestinationCurrency { get; }

    public IReadOnlyCollection<TransactionTag> Tags { get; }
    
    private ExpenseTransaction(
        Guid id, 
        DateOnly date, 
        Balance source,
        decimal amountInSourceCurrency, 
        ExpenseCategory destination,
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

    public static Result<ExpenseTransaction> Create(
        Guid id,
        DateOnly dateInUtc,
        Balance source,
        ExpenseCategory destination,
        decimal amountInSourceCurrency,
        decimal amountInDestinationCurrency,
        IEnumerable<string> tags,
        bool recalculateBalances)
    {
        if (amountInSourceCurrency <= 0 || amountInDestinationCurrency <= 0)
            return TransactionErrors.AmountShouldBeGreaterThanZero();

        var tagsResult = TransactionTag.CreateMultiple(tags);
        if (tagsResult.IsFailure) return tagsResult.Error;
        
        var transaction = new ExpenseTransaction(
            id, dateInUtc, 
            source, amountInSourceCurrency, 
            destination, amountInDestinationCurrency,
            tagsResult.Value);

        if (recalculateBalances)
        {
            transaction.Source.ApplyTransaction(transaction);
        }

        return transaction;
    }
}