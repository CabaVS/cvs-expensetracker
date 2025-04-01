namespace CabaVS.ExpenseTracker.Persistence.EfEntities;

internal sealed class BalanceEf
{
    public Guid Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime ModifiedOn { get; set; }

    public string Name { get; set; } = string.Empty;

    public Guid WorkspaceId { get; set; }
    public WorkspaceEf? Workspace { get; set; }

    public Guid CurrencyId { get; set; }
    public CurrencyEf? Currency { get; set; }
}
