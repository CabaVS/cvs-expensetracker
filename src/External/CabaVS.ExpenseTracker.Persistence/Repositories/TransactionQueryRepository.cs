using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Features.Balances.Models;
using CabaVS.ExpenseTracker.Application.Features.Categories.Models;
using CabaVS.ExpenseTracker.Application.Features.Currencies.Models;
using CabaVS.ExpenseTracker.Application.Features.Transactions.Models;
using CabaVS.ExpenseTracker.Domain.Enums;
using CabaVS.ExpenseTracker.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace CabaVS.ExpenseTracker.Persistence.Repositories;

internal sealed class TransactionQueryRepository(ApplicationDbContext dbContext) : ITransactionQueryRepository
{
    public async Task<TransactionModel[]> GetAllAsync(Guid workspaceId, CancellationToken cancellationToken = default)
    {
        Transaction[] entities = await dbContext.Transactions
            .AsNoTracking()
            .Include(x => x.SourceBalance).ThenInclude(sb => sb!.Currency)
            .Include(x => x.DestinationBalance).ThenInclude(db => db!.Currency)
            .Include(x => x.SourceCategory).ThenInclude(sc => sc!.Currency)
            .Include(x => x.DestinationCategory).ThenInclude(dc => dc!.Currency)
            .Where(x => x.WorkspaceId == workspaceId)
            .ToArrayAsync(cancellationToken);
        return entities.Select(ConvertToModel).ToArray();
    }

    public async Task<TransactionModel?> GetByIdAsync(Guid workspaceId, Guid transactionId, CancellationToken cancellationToken = default)
    {
        Transaction? entity = await dbContext.Transactions
            .AsNoTracking()
            .Include(x => x.SourceBalance).ThenInclude(sb => sb!.Currency)
            .Include(x => x.DestinationBalance).ThenInclude(db => db!.Currency)
            .Include(x => x.SourceCategory).ThenInclude(sc => sc!.Currency)
            .Include(x => x.DestinationCategory).ThenInclude(dc => dc!.Currency)
            .Where(x => x.WorkspaceId == workspaceId)
            .Where(x => x.Id == transactionId)
            .FirstOrDefaultAsync(cancellationToken);
        return entity is not null ? ConvertToModel(entity) : null;
    }

    private static Func<Transaction, TransactionModel> ConvertToModel =>
        x =>
        {
            object source = x.Type is TransactionType.Expense or TransactionType.Transfer
                ? MapToBalanceModel(x.SourceBalance!)
                : MapToCategoryModel(x.SourceCategory!);
            object destination = x.Type is TransactionType.Income or TransactionType.Transfer
                ? MapToBalanceModel(x.DestinationBalance!)
                : MapToCategoryModel(x.DestinationCategory!);

            return new TransactionModel(
                x.Id, x.CreatedOn, x.ModifiedOn, x.Date, x.Tags,
                x.AmountInSourceCurrency, x.AmountInDestinationCurrency,
                x.Type, source, destination);

            static BalanceModel MapToBalanceModel(Balance balance) =>
                new(
                    balance.Id,
                    balance.Name,
                    balance.Amount,
                    new CurrencyModel(
                        balance.Currency.Id,
                        balance.Currency.Name,
                        balance.Currency.Code,
                        balance.Currency.Symbol));

            static CategoryModel MapToCategoryModel(Category category) =>
                new(
                    category.Id,
                    category.Name,
                    category.Type,
                    new CurrencyModel(
                        category.Currency.Id,
                        category.Currency.Name,
                        category.Currency.Code,
                        category.Currency.Symbol));
        };
}
