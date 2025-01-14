using System.Linq.Expressions;
using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Features.TransferTransactions.Models;
using CabaVS.ExpenseTracker.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace CabaVS.ExpenseTracker.Persistence.Repositories;

internal sealed class TransferTransactionReadRepository(ApplicationDbContext dbContext) : ITransferTransactionReadRepository
{
    public async Task<TransferTransactionModel?> GetByIdAsync(
        Guid workspaceId, Guid transferTransactionId,
        CancellationToken cancellationToken = default)
    {
        TransferTransactionModel? model = await dbContext.TransferTransactions
            .Where(x => x.Id == transferTransactionId)
            .Where(x => x.Source.WorkspaceId == workspaceId)
            .Where(x => x.Destination.WorkspaceId == workspaceId)
            .Select(ProjectionToModel)
            .FirstOrDefaultAsync(cancellationToken);
        return model;
    }

    private static Expression<Func<TransferTransaction, TransferTransactionModel>> ProjectionToModel =>
        transferTransaction => new TransferTransactionModel(
            transferTransaction.Id,
            transferTransaction.Date,
            transferTransaction.Tags,
            transferTransaction.Amount,
            new TransferTransactionModel.CurrencyModel(
                transferTransaction.Currency.Id,
                transferTransaction.Currency.Code),
            transferTransaction.AmountInSourceCurrency,
            new TransferTransactionModel.BalanceModel(
                transferTransaction.Source.Id,
                transferTransaction.Source.Name,
                new TransferTransactionModel.CurrencyModel(
                    transferTransaction.Source.Currency.Id,
                    transferTransaction.Source.Currency.Code)),
            transferTransaction.AmountInDestinationCurrency,
            new TransferTransactionModel.BalanceModel(
                transferTransaction.Destination.Id,
                transferTransaction.Destination.Name,
                new TransferTransactionModel.CurrencyModel(
                    transferTransaction.Destination.Currency.Id,
                    transferTransaction.Destination.Currency.Code)));
}
