using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using DomainIncomeCategory = CabaVS.ExpenseTracker.Domain.Entities.IncomeCategory;

namespace CabaVS.ExpenseTracker.Infrastructure.Persistence.Repositories;

internal sealed class IncomeCategoryRepository(ApplicationDbContext dbContext) : IIncomeCategoryRepository
{
    public async Task<DomainIncomeCategory?> GetById(Guid id, Guid workspaceId, CancellationToken ct = default)
    {
        var entity = await dbContext.IncomeCategories
            .AsNoTracking()
            .Include(ic => ic.Currency)
            .Where(ic => ic.WorkspaceId == workspaceId)
            .FirstOrDefaultAsync(ic => ic.Id == id, ct);

        return entity?.ToDomain();
    }

    public async Task<Guid> Create(DomainIncomeCategory incomeCategory, Guid workspaceId, CancellationToken ct = default)
    {
        var entity = IncomeCategory.FromDomain(incomeCategory, workspaceId);

        var added = await dbContext.IncomeCategories.AddAsync(entity, ct);

        return added.Entity.Id;
    }

    public Task Update(DomainIncomeCategory incomeCategory, Guid workspaceId, CancellationToken ct = default)
    {
        var entity = IncomeCategory.FromDomain(incomeCategory, workspaceId);

        _ = dbContext.IncomeCategories.Update(entity);

        return Task.CompletedTask;
    }

    public Task Delete(DomainIncomeCategory incomeCategory, Guid workspaceId, CancellationToken ct = default)
    {
        var entity = IncomeCategory.FromDomain(incomeCategory, workspaceId);

        _ = dbContext.IncomeCategories.Remove(entity);

        return Task.CompletedTask;
    }
}