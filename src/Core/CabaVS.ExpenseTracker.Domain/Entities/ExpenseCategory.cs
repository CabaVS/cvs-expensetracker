using CabaVS.ExpenseTracker.Domain.Primitives;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Domain.ValueObjects;

namespace CabaVS.ExpenseTracker.Domain.Entities;

public sealed class ExpenseCategory : Entity
{
    public CategoryName Name { get; }
    public Currency Currency { get; }
    
    private ExpenseCategory(Guid id, CategoryName name, Currency currency) : base(id)
    {
        Name = name;
        Currency = currency;
    }
    
    public static Result<ExpenseCategory> Create(Guid id, string name, Currency currency)
    {
        var nameResult = CategoryName.Create(name);
        if (nameResult.IsFailure) return nameResult.Error;

        return new ExpenseCategory(id, nameResult.Value, currency);
    }
}