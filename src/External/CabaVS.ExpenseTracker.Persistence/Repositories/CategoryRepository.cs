using CabaVS.ExpenseTracker.Application.Contracts.Persistence.Repositories;
using CabaVS.ExpenseTracker.Domain.Entities;
using CabaVS.ExpenseTracker.Persistence.EfEntities;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CabaVS.ExpenseTracker.Persistence.Repositories;

internal sealed class CategoryRepository(ApplicationDbContext dbContext) : ICategoryRepository
{
    public Task<Guid> CreateAsync(Category category, Guid workspaceId, CancellationToken cancellationToken = default)
    {
        var entity = CategoryEf.FromDomain(category, workspaceId);
        
        EntityEntry<CategoryEf> added = dbContext.Categories.Add(entity);

        return Task.FromResult(added.Entity.Id);
    }
}
