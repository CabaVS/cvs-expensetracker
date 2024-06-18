using System.Collections.ObjectModel;
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
        List<TransactionTag>? tags = null) : base(id)
    {
        Date = date;

        Source = source;
        AmountInSourceCurrency = amountInSourceCurrency;
        
        Destination = destination;
        AmountInDestinationCurrency = amountInDestinationCurrency;

        Tags = new ReadOnlyCollection<TransactionTag>(tags ?? []);
    }

    public static Result<ExpenseTransaction> Create(
        Guid id,
        DateOnly dateInUtc,
        Balance source,
        ExpenseCategory destination,
        decimal amountInSourceCurrency,
        decimal amountInDestinationCurrency,
        IEnumerable<string>? tags = null)
    {
        if (amountInSourceCurrency <= 0 || amountInDestinationCurrency <= 0)
            return TransactionErrors.AmountShouldBeGreaterThanZero();

        var tagsResult = TryConvertTags(tags);
        if (tagsResult.IsFailure) return tagsResult.Error;
        
        source.Amount -= amountInSourceCurrency;
        
        return new ExpenseTransaction(
            id, dateInUtc, 
            source, amountInSourceCurrency, 
            destination, amountInDestinationCurrency,
            tagsResult.Value);
    }

    private static Result<List<TransactionTag>?> TryConvertTags(IEnumerable<string>? tags)
    {
        if (tags is null) return new List<TransactionTag>(0);
        
        var tagsRawList = tags.ToList();
        if (tagsRawList.Count == 0) return new List<TransactionTag>(0);
        
        var tagsResultsList = tagsRawList.Select(TransactionTag.Create).ToList();

        var firstFailed = tagsResultsList.FirstOrDefault(x => x.IsFailure);
        if (firstFailed is not null)
            return firstFailed.Error;

        var tagsList = tagsResultsList.Select(x => x.Value).ToList();
            
        var tagsGroupedByCount = tagsList
            .GroupBy(x => x)
            .ToDictionary(x => x.Key, x => x.Count());

        var (firstDuplicate, _) = tagsGroupedByCount.FirstOrDefault(x => x.Value > 1);
        if (firstDuplicate is not null)
            return TransactionErrors.TagDuplication(firstDuplicate.Value);

        return tagsList;
    }
}