using CabaVS.ExpenseTracker.Application.Features.Categories.Models;

namespace CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;

public interface IIncomeCategoryReadRepository
{
    Task<IncomeCategoryModel[]> GetAll(Guid workspaceId, CancellationToken ct = default);
    Task<IncomeCategoryModel?> GetById(Guid incomeCategoryId, Guid workspaceId, CancellationToken ct = default);
}