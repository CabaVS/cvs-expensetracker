using CabaVS.ExpenseTracker.Domain.Entities.Abstractions;
using CabaVS.ExpenseTracker.Domain.Primitives;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Domain.ValueObjects;

namespace CabaVS.ExpenseTracker.Domain.Entities;

public sealed class Balance : AuditableEntity, IWithCurrency, IWithWorkspace
{
    public BalanceName Name { get; set; }
    public decimal Amount { get; set; }
    
    public Currency Currency { get; }
    public Workspace Workspace { get; }
    
    private Balance(Guid id, DateTime createdOn, DateTime modifiedOn,
        BalanceName name, decimal amount, Currency currency, Workspace workspace) : base(id, createdOn, modifiedOn)
    {
        Name = name;
        Amount = amount;
        Currency = currency;
        Workspace = workspace;
    }
    
    public static Result<Balance> Create(string name, decimal amount, Currency currency, Workspace workspace)
    {
        DateTime utcNow = DateTime.UtcNow;
        
        return Create(Guid.NewGuid(), utcNow, utcNow, name, amount, currency, workspace);
    }

    public static Result<Balance> Create(Guid id, DateTime createdOn, DateTime modifiedOn, 
        string name, decimal amount, Currency currency, Workspace workspace)
    {
        Result<BalanceName> balanceNameResult = BalanceName.Create(name);
        if (balanceNameResult.IsFailure)
        {
            return balanceNameResult.Error;
        }
        
        var balance = new Balance(id, createdOn, modifiedOn, balanceNameResult.Value, amount, currency, workspace);
        return balance;
    }
}
