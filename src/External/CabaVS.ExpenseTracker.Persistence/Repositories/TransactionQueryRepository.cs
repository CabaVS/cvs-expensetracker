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

    public async Task<TransactionMoneyModel[]> GetTransactionsMoneyAsync(
        Guid workspaceId, TransactionType type, DateOnly from, DateOnly to,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Transaction> query = dbContext.Transactions
            .Where(x => x.WorkspaceId == workspaceId)
            .Where(x => x.Type == type)
            .Where(x => from <= x.Date && x.Date <= to);
        
        IQueryable<TransactionMoneyModel> final = type switch
        {
            TransactionType.Income => query.Select(
                x => new TransactionMoneyModel(
                    x.AmountInDestinationCurrency, x.DestinationBalance!.Currency.Code)),
            TransactionType.Expense => query.Select(
                x => new TransactionMoneyModel(
                    x.AmountInSourceCurrency, x.SourceBalance!.Currency.Code)),
            _ => throw new InvalidOperationException()
        };
        
        return await final.ToArrayAsync(cancellationToken);
    }

    public async Task<TransactionMoneyByCategoryModel[]> GetTransactionsMoneyByCategoryAsync(
        Guid workspaceId, TransactionType type, DateOnly from, DateOnly to,
        CancellationToken cancellationToken = default)
    {
        Transaction[] transactions = await dbContext.Transactions
            .AsNoTracking()
            .Include(x => x.SourceCategory)
            .ThenInclude(x => x!.Currency)
            .Include(x => x.DestinationCategory)
            .ThenInclude(x => x!.Currency)
            .Where(x => x.WorkspaceId == workspaceId)
            .Where(x => x.Type == type)
            .Where(x => from <= x.Date && x.Date <= to)
            .ToArrayAsync(cancellationToken);
        if (transactions.Length == 0)
        {
            return [];
        }

        var groupedByCategory = type switch
        {
            TransactionType.Income => transactions
                .Where(x => x.SourceCategory is not null)
                .GroupBy(x => (x.SourceCategory!.Id, x.SourceCategory.Name, x.SourceCategory.Currency.Code))
                .Select(g => new
                {
                    CategoryId = g.Key.Id,
                    CategoryName = g.Key.Name,
                    CategoryCurrencyCode = g.Key.Code,
                    Transactions = g.Select(x => new
                    {
                        Amount = x.AmountInSourceCurrency,
                        x.Tags,
                        g.Key.Code
                    })
                }),
            TransactionType.Expense => transactions
                .Where(x => x.DestinationCategory is not null)
                .GroupBy(x => (x.DestinationCategory!.Id, x.DestinationCategory.Name, x.DestinationCategory.Currency.Code))
                .Select(g => new
                {
                    CategoryId = g.Key.Id,
                    CategoryName = g.Key.Name,
                    CategoryCurrencyCode = g.Key.Code,
                    Transactions = g.Select(x => new
                    {
                        Amount = x.AmountInDestinationCurrency,
                        x.Tags,
                        g.Key.Code
                    })
                }),
            _ => throw new InvalidOperationException()
        };

        TransactionMoneyByCategoryModel[] resultModels = groupedByCategory
            .Select(x => new TransactionMoneyByCategoryModel(
                x.CategoryId, x.CategoryName, x.Transactions.Sum(y => y.Amount), x.CategoryCurrencyCode)
            {
                ByTag = x.Transactions
                    .GroupBy(y => string.Join(',', y.Tags.OrderBy(z => z)))
                    .Select(g =>
                    {
                        var first = g.First();
                        
                        return new TransactionMoneyByTagModel(
                            first.Tags,
                            g.Sum(y => y.Amount),
                            first.Code);
                    })
                    .OrderByDescending(y => y.Amount)
                    .ToArray()
            })
            .OrderByDescending(x => x.Amount)
            .ToArray();

        return resultModels;
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
