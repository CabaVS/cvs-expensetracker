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
        if (string.IsNullOrWhiteSpace(value)) return WorkspaceErrors.NameIsNullOrWhitespace();
        if (value.Length > MaxLength) return WorkspaceErrors.NameTooLong(value);
        if (value.Contains(',')) return TransactionErrors.TagContainsComma(value);
        
        return new TransactionTag(value);
    }
}