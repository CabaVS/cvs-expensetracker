using System.Collections.ObjectModel;
using System.Reflection;
using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Primitives;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Domain.ValueObjects;

namespace CabaVS.ExpenseTracker.Domain.Entities;

public sealed class ExpenseTransaction : Transaction<Balance, Category>
{
    private ExpenseTransaction(
        Guid id, TransactionType type, DateOnly date,
        Balance source, decimal amountInSourceCurrency,
        Category destination, decimal amountInDestinationCurrency,
        List<TransactionTag>? tags = null) : base(
        id, type, date,
        source, amountInSourceCurrency, 
        destination, amountInDestinationCurrency,
        tags) {  }

    public static Result<ExpenseTransaction> Create(
        Guid id, DateOnly date,
        Balance source, decimal amountInSourceCurrency, 
        Category destination, decimal amountInDestinationCurrency,
        IEnumerable<string>? tags = null)
    {
        if (destination.Type != CategoryType.Expense)
            return TransactionErrors.CategoryTypeAndTransactionTypeMismatch(destination.Type, TransactionType.Expense);
        
        var creationResult = CreateTransaction<ExpenseTransaction>(
            id, TransactionType.Expense, date,
            source, amountInSourceCurrency, 
            destination, amountInDestinationCurrency,
            tags);
        if (creationResult.IsFailure) return creationResult.Value;
        
        source.Amount -= amountInSourceCurrency;

        return creationResult.Value;
    }
}

public sealed class IncomeTransaction : Transaction<Category, Balance>
{
    private IncomeTransaction(
        Guid id, TransactionType type, DateOnly date,
        Category source, decimal amountInSourceCurrency,
        Balance destination, decimal amountInDestinationCurrency,
        List<TransactionTag>? tags = null) : base(
        id, type, date,
        source, amountInSourceCurrency, 
        destination, amountInDestinationCurrency,
        tags) {  }

    public static Result<IncomeTransaction> Create(
        Guid id, DateOnly date,
        Category source, decimal amountInSourceCurrency, 
        Balance destination, decimal amountInDestinationCurrency,
        IEnumerable<string>? tags = null)
    {
        if (source.Type != CategoryType.Income)
            return TransactionErrors.CategoryTypeAndTransactionTypeMismatch(source.Type, TransactionType.Income);
        
        var creationResult = CreateTransaction<IncomeTransaction>(
            id, TransactionType.Income, date,
            source, amountInSourceCurrency, 
            destination, amountInDestinationCurrency,
            tags);
        if (creationResult.IsFailure) return creationResult.Value;
        
        destination.Amount += amountInDestinationCurrency;

        return creationResult.Value;
    }
}

public sealed class TransferTransaction : Transaction<Balance, Balance>
{
    private TransferTransaction(
        Guid id, TransactionType type, DateOnly date,
        Balance source, decimal amountInSourceCurrency,
        Balance destination, decimal amountInDestinationCurrency,
        List<TransactionTag>? tags = null) : base(
            id, type, date,
            source, amountInSourceCurrency, 
            destination, amountInDestinationCurrency,
            tags)
    {
    }

    public static Result<TransferTransaction> Create(
        Guid id, DateOnly date,
        Balance source, decimal amountInSourceCurrency, 
        Balance destination, decimal amountInDestinationCurrency,
        IEnumerable<string>? tags = null)
    {
        if (source == destination)
            return TransactionErrors.SourceAndDestinationShouldDiffer();
        
        var creationResult = CreateTransaction<TransferTransaction>(
            id, TransactionType.Transfer, date,
            source, amountInSourceCurrency, 
            destination, amountInDestinationCurrency,
            tags);
        if (creationResult.IsFailure) return creationResult.Value;

        source.Amount -= amountInSourceCurrency;
        destination.Amount += amountInDestinationCurrency;

        return creationResult.Value;
    }
}

public abstract class Transaction<TSource, TDestination> : Entity
    where TSource : Entity
    where TDestination : Entity
{
    public TransactionType Type { get; }
    public DateOnly Date { get; }
    
    public TSource Source { get; }
    public decimal AmountInSourceCurrency { get; }
    
    public TDestination Destination { get; }
    public decimal AmountInDestinationCurrency { get; }
    
    public IReadOnlyCollection<TransactionTag> Tags { get; }
    
    protected Transaction(
        Guid id,
        TransactionType type,
        DateOnly date,
        TSource source,
        decimal amountInSourceCurrency,
        TDestination destination,
        decimal amountInDestinationCurrency,
        List<TransactionTag>? tags = null) : base(id)
    {
        Type = type;
        Date = date;
        Source = source;
        AmountInSourceCurrency = amountInSourceCurrency;
        Destination = destination;
        AmountInDestinationCurrency = amountInDestinationCurrency;
        Tags = new ReadOnlyCollection<TransactionTag>(tags ?? []);
    }
    
    protected static Result<TTransaction> CreateTransaction<TTransaction>(
        Guid id,
        TransactionType type,
        DateOnly dateInUtc,
        TSource source,
        decimal amountInSourceCurrency,
        TDestination destination,
        decimal amountInDestinationCurrency,
        IEnumerable<string>? tags = null) where TTransaction : Transaction<TSource, TDestination>
    {
        if (amountInSourceCurrency <= 0 || amountInDestinationCurrency <= 0)
            return TransactionErrors.AmountShouldBeGreaterThanZero();

        var tagsResult = TransactionTag.CreateMultiple(tags);
        if (tagsResult.IsFailure) return tagsResult.Error;

        return (TTransaction)Activator.CreateInstance(
            typeof(TTransaction),
            BindingFlags.Instance | BindingFlags.NonPublic,
            null,
            [id, type, dateInUtc, source, amountInSourceCurrency,
                destination, amountInDestinationCurrency, tagsResult.Value],
            null)!;
    }
}

public enum TransactionType
{
    Expense,
    Income,
    Transfer
}