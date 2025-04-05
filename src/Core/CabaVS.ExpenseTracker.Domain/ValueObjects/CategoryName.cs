using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Primitives;
using CabaVS.ExpenseTracker.Domain.Shared;

namespace CabaVS.ExpenseTracker.Domain.ValueObjects;

public sealed class CategoryName : ValueObject
{
    public const int MaxLength = 50;
    
    public string Value { get; }

    private CategoryName(string value) => 
        Value = value;

    protected override IEnumerable<object> AtomicValues
    {
        get
        {
            yield return Value;
        }
    }

    public static Result<CategoryName> Create(string categoryName) =>
        Result<string>.Success(categoryName)
            .EnsureStringNotNullOrWhitespace(CategoryErrors.NameIsNullOrWhitespace())
            .EnsureStringNotTooLong(MaxLength, CategoryErrors.NameIsTooLong(categoryName))
            .Map(x => new CategoryName(x));
}
