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

    public static Result<WorkspaceName> Create(string value)
    {
        return Result<string>.Success(value)
            .EnsureStringNotNullOrWhitespace(WorkspaceErrors.NameIsNullOrWhitespace())
            .EnsureStringNotTooLong(MaxLength, WorkspaceErrors.NameTooLong(value))
            .Map(x => new WorkspaceName(x));
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}