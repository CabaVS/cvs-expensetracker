using CabaVS.ExpenseTracker.Application.Contracts.Persistence.Repositories;
using CabaVS.ExpenseTracker.Domain.Entities;
using CabaVS.ExpenseTracker.Persistence.EfEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CabaVS.ExpenseTracker.Persistence.Repositories;

internal sealed class CategoryRepository(ApplicationDbContext dbContext) : ICategoryRepository
{
    public async Task<Category?> GetByIdAsync(Guid workspaceId, Guid categoryId, CancellationToken cancellationToken = default)
    {
        Category? category = await dbContext.Categories
            .Where(x => x.Id == categoryId)
            .Where(x => x.WorkspaceId == workspaceId)
            .Select(CategoryEf.ProjectToDomain)
            .FirstOrDefaultAsync(cancellationToken);
        return category;
    }

    public Task<Guid> CreateAsync(Category category, Guid workspaceId, CancellationToken cancellationToken = default)
    {
        var entity = CategoryEf.FromDomain(category, workspaceId);
        
        EntityEntry<CategoryEf> added = dbContext.Categories.Add(entity);

        return Task.FromResult(added.Entity.Id);
    }

    public Task UpdateAsync(Category category, Guid workspaceId, CancellationToken cancellationToken = default)
    {
        var entity = CategoryEf.FromDomain(category, workspaceId);
        
        _ = dbContext.Categories.Update(entity);

        return Task.CompletedTask;
    }
}
