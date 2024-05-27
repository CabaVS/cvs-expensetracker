using CabaVS.ExpenseTracker.Domain.Primitives;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Domain.ValueObjects;

namespace CabaVS.ExpenseTracker.Domain.Entities;

public sealed class Balance : Entity
{
    public BalanceName Name { get; }
    public Money Money { get; }
    
    private Balance(Guid id, BalanceName name, Money money) : base(id)
    {
        Name = name;
        Money = money;
    }
    
    public static Result<Balance> Create(Guid id, string name, decimal amount, Currency currency)
    {
        var nameResult = BalanceName.Create(name);
        if (nameResult.IsFailure) return nameResult.Error;

        return new Balance(id, nameResult.Value, new Money(amount, currency));
    }
}