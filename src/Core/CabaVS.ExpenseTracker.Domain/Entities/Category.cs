using CabaVS.ExpenseTracker.Domain.Primitives;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Domain.ValueObjects;

namespace CabaVS.ExpenseTracker.Domain.Entities;

public sealed class Category : Entity
{
    public CategoryType Type { get; }
    public CategoryName Name { get; }
    public Currency Currency { get; }
    
    private Category(Guid id, CategoryType type, CategoryName name, Currency currency) : base(id)
    {
        Type = type;
        Name = name;
        Currency = currency;
    }
    
    public static Result<Category> Create(Guid id, CategoryType type, string name, Currency currency)
    {
        var nameResult = CategoryName.Create(name);
        if (nameResult.IsFailure) return nameResult.Error;

        return new Category(id, type, nameResult.Value, currency);
    }
}

public enum CategoryType
{
    Expense,
    Income
}