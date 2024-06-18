using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Primitives;
using CabaVS.ExpenseTracker.Domain.Shared;

namespace CabaVS.ExpenseTracker.Domain.ValueObjects;

public sealed class TransactionTag : ValueObject
{
    public const int MaxLength = 16;
    
    public string Value { get; }

    private TransactionTag(string value)
    {
        Value = value;
    }
    
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
    
    public static Result<TransactionTag> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return TransactionErrors.TagIsNullOrWhitespace();
        if (value.Length > MaxLength) return TransactionErrors.TagTooLong(value);
        if (value.Contains(',')) return TransactionErrors.TagContainsComma(value);
        
        return new TransactionTag(value);
    }

    public static Result<List<TransactionTag>> CreateMultiple(IEnumerable<string>? values)
    {
        if (values is null) return new List<TransactionTag>(0);
        
        var tagsRawList = values.ToList();
        if (tagsRawList.Count == 0) return new List<TransactionTag>(0);
        
        var tagsResultsList = tagsRawList.Select(Create).ToList();

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