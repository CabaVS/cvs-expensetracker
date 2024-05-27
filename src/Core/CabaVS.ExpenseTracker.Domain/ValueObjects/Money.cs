using CabaVS.ExpenseTracker.Domain.Entities;
using CabaVS.ExpenseTracker.Domain.Primitives;

namespace CabaVS.ExpenseTracker.Domain.ValueObjects;

public sealed class Money(decimal Amount, Currency Currency) : ValueObject
{
    public decimal Amount { get; }
    public Currency Currency { get; }
    
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Amount;
        yield return Currency;
    }
}