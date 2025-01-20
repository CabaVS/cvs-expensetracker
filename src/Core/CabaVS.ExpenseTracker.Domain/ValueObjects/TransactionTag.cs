using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Primitives;
using CabaVS.ExpenseTracker.Domain.Shared;

namespace CabaVS.ExpenseTracker.Domain.ValueObjects;

public sealed class TransactionTag : ValueObject
{
    public const int MaxLength = 16;
    
    public string Value { get; }

    private TransactionTag(string value) => Value = value;
    
    public static Result<TransactionTag> Create(string value) =>
        Result<string>.Success(value)
            .EnsureStringNotNullOrWhitespace(TransactionErrors.TransactionTagIsNullOrWhitespace())
            .EnsureStringNotTooLong(MaxLength, TransactionErrors.TransactionTagIsTooLong(value))
            .Ensure(tag => tag.Contains(',', StringComparison.InvariantCulture), TransactionErrors.TransactionTagContainsComma())
            .Map(x => new TransactionTag(x));

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
