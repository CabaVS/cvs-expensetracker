using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Primitives;
using CabaVS.ExpenseTracker.Domain.Shared;

namespace CabaVS.ExpenseTracker.Domain.ValueObjects;

public sealed class BalanceName : ValueObject
{
    public const int MaxLength = 32;
    
    public string Value { get; }

    private BalanceName(string value)
    {
        Value = value;
    }
    
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
    
    public static Result<BalanceName> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return BalanceErrors.NameIsNullOrWhitespace();
        if (value.Length > MaxLength) return BalanceErrors.NameTooLong(value);
        
        return new BalanceName(value);
    }
}