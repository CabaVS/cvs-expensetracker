namespace CabaVS.ExpenseTracker.Persistence.Entities.Abstractions;

public interface IAuditableEntity
{
    public Guid Id { get; set; }
    
    DateTime CreatedOn { get; set; }
    DateTime? ModifiedOn { get; set; }
}