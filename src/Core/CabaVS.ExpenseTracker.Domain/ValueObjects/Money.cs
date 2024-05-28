using CabaVS.ExpenseTracker.Domain.Entities;
using CabaVS.ExpenseTracker.Domain.Primitives;

namespace CabaVS.ExpenseTracker.Domain.ValueObjects;

public sealed class Money(decimal amount, Currency currency) : ValueObject
{
    public decimal Amount { get; } = amount;
    public Currency Currency { get; } = currency;
    
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Amount;
        yield return Currency;
    }
}