using CabaVS.ExpenseTracker.Domain.Entities;
using CabaVS.ExpenseTracker.Domain.Enums;

namespace CabaVS.ExpenseTracker.Persistence.EfEntities;

internal sealed class CategoryEf
{
    public Guid Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime ModifiedOn { get; set; }

    public string Name { get; set; } = string.Empty;
    public CategoryType Type { get; set; }

    public Guid WorkspaceId { get; set; }
    public WorkspaceEf? Workspace { get; set; }

    public Guid CurrencyId { get; set; }
    public CurrencyEf? Currency { get; set; }
    
    internal static CategoryEf FromDomain(Category category, Guid workspaceId) => new()
    {
        Id = category.Id,
        CreatedOn = category.CreatedOn,
        ModifiedOn = category.ModifiedOn,
        Name = category.Name.Value,
        Type = category.Type,
        WorkspaceId = workspaceId,
        CurrencyId = category.Currency.Id
    };
}
