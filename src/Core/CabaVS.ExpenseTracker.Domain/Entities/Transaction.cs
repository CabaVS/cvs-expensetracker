using CabaVS.ExpenseTracker.Domain.Abstractions;
using CabaVS.ExpenseTracker.Domain.Enums;
using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Primitives;
using CabaVS.ExpenseTracker.Domain.Shared;

namespace CabaVS.ExpenseTracker.Domain.Entities;

public sealed class Transaction : AuditableEntity
{
    public DateOnly Date { get; }
    public TransactionType Type { get; }
    public HashSet<string> Tags { get; }
    
    public decimal AmountInSourceCurrency { get; }
    public decimal AmountInDestinationCurrency { get; }
    
    public IWithCurrency Source { get; }
    public IWithCurrency Destination { get; }
    
    private Transaction(
        Guid id, DateTime createdOn, DateTime modifiedOn,
        DateOnly date, TransactionType type, HashSet<string> tags,
        decimal amountInSourceCurrency, decimal amountInDestinationCurrency,
        IWithCurrency source, IWithCurrency destination) : base(id, createdOn, modifiedOn)
    {
        Date = date;
        Type = type;
        Tags = tags;
        AmountInSourceCurrency = amountInSourceCurrency;
        AmountInDestinationCurrency = amountInDestinationCurrency;
        Source = source;
        Destination = destination;
    }

    public static Result<Transaction> CreateNew(
        DateOnly date, TransactionType type, IEnumerable<string> tags,
        decimal amountInSourceCurrency, decimal amountInDestinationCurrency,
        IWithCurrency source, IWithCurrency destination)
    {
        DateTime utcNow = DateTime.UtcNow;

        return CreateExisting(
            Guid.NewGuid(), utcNow, utcNow,
            date, type, tags,
            amountInSourceCurrency, amountInDestinationCurrency,
            source, destination, true);
    }

    public static Result<Transaction> CreateExisting(
        Guid id, DateTime createdOn, DateTime modifiedOn,
        DateOnly date, TransactionType type, IEnumerable<string> tags,
        decimal amountInSourceCurrency, decimal amountInDestinationCurrency,
        IWithCurrency source, IWithCurrency destination, bool recalculateAmounts = false)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(destination);
        
        var tagsSet = new HashSet<string>(tags.OrderBy(x => x));

        if (source == destination)
        {
            return TransactionErrors.SourceAndDestinationAreSame();
        }

        switch (type)
        {
            case TransactionType.Expense when 
                source is not Balance || destination is not Category { Type: CategoryType.Expense }:
            case TransactionType.Income when
                source is not Category { Type: CategoryType.Income } || destination is not Balance:
            case TransactionType.Transfer when
                source is not Balance || destination is not Balance:
                return TransactionErrors.SourceOrDestinationMismatch();
        }

        if (source.Currency != destination.Currency &&
            amountInSourceCurrency == amountInDestinationCurrency)
        {
            return TransactionErrors.AmountsMustDifferWhenCurrenciesAreDifferent();
        }
        
        var transaction = new Transaction(
            id, createdOn, modifiedOn,
            date, type, tagsSet, 
            amountInSourceCurrency, amountInDestinationCurrency,
            source, destination);
        if (recalculateAmounts)
        {
            if (source is Balance sourceBalance)
            {
                sourceBalance.Amount -= amountInSourceCurrency;
            }

            if (destination is Balance destinationBalance)
            {
                destinationBalance.Amount += amountInDestinationCurrency;
            }
        }
        
        return transaction;
    }
    
    public void Rollback()
    {
        if (Source is Balance sourceBalance)
        {
            sourceBalance.Amount += AmountInSourceCurrency;
        }

        if (Destination is Balance destinationBalance)
        {
            destinationBalance.Amount -= AmountInDestinationCurrency;
        }
    }
}
