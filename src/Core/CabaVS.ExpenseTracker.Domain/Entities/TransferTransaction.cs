using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Primitives;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Domain.ValueObjects;

namespace CabaVS.ExpenseTracker.Domain.Entities;

public sealed class TransferTransaction : Entity
{
    public DateOnly Date { get; private set; }
    public TransactionTag[] Tags { get; private set; }
    
    public decimal Amount { get; }
    public Currency Currency { get; }

    public decimal AmountInSourceCurrency { get; }
    public Balance Source { get; }
    
    public decimal AmountInDestinationCurrency { get; }
    public Balance Destination { get; }
    
    private TransferTransaction(
        Guid id,
        DateTime createdOn,
        DateTime? modifiedOn,
        DateOnly date,
        TransactionTag[] tags,
        decimal amount,
        Currency currency,
        decimal amountInSourceCurrency,
        Balance source,
        decimal amountInDestinationCurrency,
        Balance destination) : base(id, createdOn, modifiedOn)
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
        DateOnly date,
        string[] tags,
        decimal amount,
        Currency currency,
        decimal amountInSourceCurrency,
        Balance source,
        decimal amountInDestinationCurrency,
        Balance destination) =>
        Create(
            Guid.NewGuid(),
            DateTime.UtcNow,
            null,
            date,
            tags,
            amount,
            currency,
            amountInSourceCurrency,
            source,
            amountInDestinationCurrency,
            destination,
            true);
    
    public static Result<TransferTransaction> Create(
        Guid id,
        DateTime createdOn,
        DateTime? modifiedOn,
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
        ArgumentNullException.ThrowIfNull(tags);
        
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

        TransactionTag[] transactionTags = [];
        if (tags.Length > 0)
        {
            Result<TransactionTag>[] tagsResults = tags.Select(TransactionTag.Create).ToArray();
            
            Result<TransactionTag>? firstFailedResult = tagsResults.FirstOrDefault(r => r.IsFailure);
            if (firstFailedResult is not null)
            {
                return firstFailedResult.Error;
            }
            
            transactionTags = tagsResults.Select(r => r.Value).ToArray();
        }
        
        var transferTransaction = new TransferTransaction(
            id,
            createdOn,
            modifiedOn,
            date,
            transactionTags,
            amount,
            currency,
            amountInSourceCurrency,
            source,
            amountInDestinationCurrency,
            destination);
        if (recalculateSourceAndDestination)
        {
            source.ApplyTransferTransaction(transferTransaction, true);
            destination.ApplyTransferTransaction(transferTransaction, false);
        }
        
        return transferTransaction;
    }
}
