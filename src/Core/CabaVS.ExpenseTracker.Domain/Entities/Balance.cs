using CabaVS.ExpenseTracker.Domain.Primitives;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Domain.ValueObjects;

namespace CabaVS.ExpenseTracker.Domain.Entities;

public sealed class Balance : AuditableEntity
{
    public BalanceName Name { get; set; }
    public Currency Currency { get; set; }
    
    private Balance(
        Guid id, DateTime createdOn, DateTime modifiedOn,
        BalanceName name, Currency currency) : base(id, createdOn, modifiedOn)
    {
        Name = name;
        Currency = currency;
    }
    
    public static Result<Balance> CreateNew(string name, Currency currency)
    {
        DateTime utcNow = DateTime.UtcNow;
        
        return CreateExisting(
            Guid.NewGuid(), utcNow, utcNow,
            name, currency);
    }

    public static Result<Balance> CreateExisting(
        Guid id, DateTime createdOn, DateTime modifiedOn,
        string name, Currency currency) =>
        BalanceName.Create(name)
            .Map(x => new Balance(id, createdOn, modifiedOn, x, currency));
}
