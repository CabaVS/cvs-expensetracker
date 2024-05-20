using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Primitives;
using CabaVS.ExpenseTracker.Domain.Shared;

namespace CabaVS.ExpenseTracker.Domain.ValueObjects;

public sealed class WorkspaceName : ValueObject
{
    public const int MaxLength = 32;
    
    public string Value { get; }

    private WorkspaceName(string value)
    {
        Value = value;
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    public static Result<WorkspaceName> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return WorkspaceErrors.NameIsNullOrWhitespace();
        if (value.Length > MaxLength) return WorkspaceErrors.NameTooLong(value);
        
        return new WorkspaceName(value);
    }
}