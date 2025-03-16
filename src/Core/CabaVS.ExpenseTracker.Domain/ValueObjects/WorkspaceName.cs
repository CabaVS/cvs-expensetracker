using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Primitives;
using CabaVS.ExpenseTracker.Domain.Shared;

namespace CabaVS.ExpenseTracker.Domain.ValueObjects;

public sealed class WorkspaceName : ValueObject
{
    public const int MaxLength = 50;
    
    public string Value { get; }

    private WorkspaceName(string value)
    {
        Value = value;
    }

    protected override IEnumerable<object> AtomicValues
    {
        get
        {
            yield return Value;
        }
    }

    public static Result<WorkspaceName> Create(string workspaceName) =>
        Result<string>.Success(workspaceName)
            .EnsureStringNotNullOrWhitespace(WorkspaceErrors.WorkspaceNameIsNullOrWhitespace())
            .EnsureStringNotTooLong(MaxLength, WorkspaceErrors.WorkspaceNameIsTooLong(workspaceName))
            .Map(x => new WorkspaceName(x));
}