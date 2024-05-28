using CabaVS.ExpenseTracker.Domain.Primitives;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Domain.ValueObjects;

namespace CabaVS.ExpenseTracker.Domain.Entities;

public sealed class IncomeCategory : Entity
{
    public CategoryName Name { get; }
    public Currency Currency { get; }
    
    private IncomeCategory(Guid id, CategoryName name, Currency currency) : base(id)
    {
        Name = name;
        Currency = currency;
    }
    
    public static Result<IncomeCategory> Create(Guid id, string name, Currency currency)
    {
        var nameResult = CategoryName.Create(name);
        if (nameResult.IsFailure) return nameResult.Error;

        return new IncomeCategory(id, nameResult.Value, currency);
    }
}