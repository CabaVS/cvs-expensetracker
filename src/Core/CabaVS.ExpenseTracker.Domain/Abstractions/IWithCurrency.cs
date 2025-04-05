using CabaVS.ExpenseTracker.Domain.Entities;

namespace CabaVS.ExpenseTracker.Domain.Abstractions;

public interface IWithCurrency : IAuditableEntity
{
    Currency Currency { get; }
}
