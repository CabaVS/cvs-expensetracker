using CabaVS.ExpenseTracker.Domain.Enums;
using CabaVS.ExpenseTracker.Domain.Primitives;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Domain.ValueObjects;

namespace CabaVS.ExpenseTracker.Domain.Entities;

public sealed class Category : AuditableEntity
{
    public CategoryName Name { get; }
    public CategoryType Type { get; }
    public Currency Currency { get; }
    
    private Category(
        Guid id, DateTime createdOn, DateTime modifiedOn,
        CategoryName name, CategoryType type, Currency currency) : base(id, createdOn, modifiedOn)
    {
        Name = name;
        Type = type;
        Currency = currency;
    }
    
    public static Result<Category> CreateNew(string name, CategoryType type, Currency currency)
    {
        DateTime utcNow = DateTime.UtcNow;
        
        return CreateExisting(
            Guid.NewGuid(), utcNow, utcNow,
            name, type, currency);
    }

    public static Result<Category> CreateExisting(
        Guid id, DateTime createdOn, DateTime modifiedOn,
        string name, CategoryType type, Currency currency) =>
        CategoryName.Create(name)
            .Map(x => new Category(id, createdOn, modifiedOn, x, type, currency));
}
