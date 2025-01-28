using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Primitives;
using CabaVS.ExpenseTracker.Domain.Shared;

namespace CabaVS.ExpenseTracker.Domain.ValueObjects;

public sealed class CategoryName : ValueObject
{
    public const int MaxLength = 32;
    
    public string Value { get; }

    private CategoryName(string value) => Value = value;

    public static Result<CategoryName> Create(string value) =>
        Result<string>.Success(value)
            .EnsureStringNotNullOrWhitespace(CategoryErrors.NameIsNullOrWhitespace())
            .EnsureStringNotTooLong(MaxLength, CategoryErrors.NameIsTooLong(value))
            .Map(x => new CategoryName(x));

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
