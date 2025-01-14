using CabaVS.ExpenseTracker.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CabaVS.ExpenseTracker.Persistence.Interceptors;

internal sealed class RecommendedTagsCreationInterceptor : ISaveChangesInterceptor
{
    public async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new())
    {
        if (eventData.Context is not ApplicationDbContext context)
        {
            throw new InvalidOperationException("Expected to have DbContext to work with.");
        }

        await HandleTransferTransactions();
        
        return result;

        async Task HandleTransferTransactions()
        {
            EntityEntry<TransferTransaction>[] addedTransferTransactions = context.ChangeTracker
                .Entries<TransferTransaction>()
                .Where(x => x.State == EntityState.Added)
                .DistinctBy(x => x.Entity.Id)
                .ToArray();
            if (addedTransferTransactions.Length == 0)
            {
                return;
            }

            var uniqueTags = addedTransferTransactions
                .SelectMany(x => x.Entity.Tags)
                .Distinct()
                .ToArray();
            if (uniqueTags.Length == 0)
            {
                return;
            }

            Guid workspaceId = addedTransferTransactions
                .Select(x => x.Entity.Source.WorkspaceId)
                .Concat(
                    addedTransferTransactions
                        .Select(x => x.Entity.Destination.WorkspaceId))
                .Distinct()
                .Single();
            var existingTags = await context.RecommendedTags
                .Where(x => x.WorkspaceId == workspaceId)
                .Where(x => x.Type == RecommendedTag.TagType.TransferTransaction)
                .Select(x => x.Name)
                .ToArrayAsync(cancellationToken);
            
            var tagsToAdd = uniqueTags.Except(existingTags).ToArray();
            if (tagsToAdd.Length == 0)
            {
                return;
            }

            context.RecommendedTags.AddRange(
                tagsToAdd.Select(
                    x => new RecommendedTag
                    {
                        Name = x,
                        Type = RecommendedTag.TagType.TransferTransaction,
                        WorkspaceId = workspaceId
                    }));
        }
    }
}
