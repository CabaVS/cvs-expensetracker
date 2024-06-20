using CabaVS.ExpenseTracker.Domain.Primitives;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Domain.ValueObjects;

namespace CabaVS.ExpenseTracker.Domain.Entities;

public sealed class Balance : Entity
{
    public BalanceName Name { get; set; }
    public decimal Amount { get; set; }
    public Currency Currency { get; }
    
    private Balance(Guid id, BalanceName name, decimal amount, Currency currency) : base(id)
    {
        Name = name;
        Amount = amount;
        Currency = currency;
    }
    
    public static Result<Balance> Create(Guid id, string name, decimal amount, Currency currency)
    {
        var nameResult = BalanceName.Create(name);
        if (nameResult.IsFailure) return nameResult.Error;

        return new Balance(id, nameResult.Value, amount, currency);
    }
}