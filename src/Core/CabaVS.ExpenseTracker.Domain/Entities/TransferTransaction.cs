using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Primitives;
using CabaVS.ExpenseTracker.Domain.Shared;

namespace CabaVS.ExpenseTracker.Domain.Entities;

public sealed class TransferTransaction : Entity
{
    public DateOnly Date { get; set; }
    public string[] Tags { get; set; }
    
    public decimal Amount { get; }
    public Currency Currency { get; }

    public decimal AmountInSourceCurrency { get; }
    public Balance Source { get; }
    
    public decimal AmountInDestinationCurrency { get; }
    public Balance Destination { get; }
    
    private TransferTransaction(
        Guid id,
        DateOnly date,
        string[] tags,
        decimal amount,
        Currency currency,
        decimal amountInSourceCurrency,
        Balance source,
        decimal amountInDestinationCurrency,
        Balance destination) : base(id)
    {
        Date = date;
        Tags = tags;
        Amount = amount;
        Currency = currency;
        AmountInSourceCurrency = amountInSourceCurrency;
        Source = source;
        AmountInDestinationCurrency = amountInDestinationCurrency;
        Destination = destination;
    }
    
    public static Result<TransferTransaction> Create(
        Guid id,
        DateOnly date,
        string[] tags,
        decimal amount,
        Currency currency,
        decimal amountInSourceCurrency,
        Balance source,
        decimal amountInDestinationCurrency,
        Balance destination,
        bool recalculateSourceAndDestination)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(destination);
        
        if (amount < 0)
        {
            return TransactionErrors.AmountShouldBePositive<TransferTransaction>(amount, nameof(amount));
        }

        if (amountInSourceCurrency < 0)
        {
            return TransactionErrors.AmountShouldBePositive<TransferTransaction>(amountInSourceCurrency, nameof(amountInSourceCurrency));
        }

        if (amountInDestinationCurrency < 0)
        {
            return TransactionErrors.AmountShouldBePositive<TransferTransaction>(amountInDestinationCurrency, nameof(amountInDestinationCurrency));
        }

        if (source == destination)
        {
            return TransactionErrors.SourceAndDestinationAreSame<TransferTransaction>();
        }

        if (recalculateSourceAndDestination)
        {
            source.Amount -= amountInSourceCurrency;
            destination.Amount += amountInDestinationCurrency;
        }
        
        return new TransferTransaction(
            id,
            date,
            tags,
            amount,
            currency,
            amountInSourceCurrency,
            source,
            amountInDestinationCurrency,
            destination);
    }
}
