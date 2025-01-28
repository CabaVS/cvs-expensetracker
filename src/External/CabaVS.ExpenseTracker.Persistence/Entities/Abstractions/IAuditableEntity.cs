namespace CabaVS.ExpenseTracker.Persistence.Entities.Abstractions;

public interface IAuditableEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime ModifiedOn { get; set; }
}
