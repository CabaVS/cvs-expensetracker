using CabaVS.ExpenseTracker.Domain.Entities;
using CabaVS.ExpenseTracker.Domain.Primitives;

namespace CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;

public interface ITransactionRepository
{
    Task<Guid> Create<TSource, TDestination>(Transaction<TSource, TDestination> transaction,
        Guid workspaceId, CancellationToken ct = default)
        where TSource : Entity where TDestination : Entity;
}