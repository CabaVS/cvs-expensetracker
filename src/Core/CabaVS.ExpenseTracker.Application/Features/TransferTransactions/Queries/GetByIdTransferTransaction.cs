using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Common.Requests;
using CabaVS.ExpenseTracker.Application.Features.TransferTransactions.Models;
using CabaVS.ExpenseTracker.Domain.Entities;
using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.TransferTransactions.Queries;

public sealed record GetByIdTransferTransactionQuery(
    Guid WorkspaceId, 
    Guid TransferTransactionId) : IWorkspaceBoundRequest, IRequest<Result<TransferTransactionModel>>;

internal sealed class GetByIdTransferTransactionQueryHandler(
    ITransferTransactionReadRepository transferTransactionReadRepository) : IRequestHandler<GetByIdTransferTransactionQuery, Result<TransferTransactionModel>>
{
    public async Task<Result<TransferTransactionModel>> Handle(
        GetByIdTransferTransactionQuery request,
        CancellationToken cancellationToken)
    {
        var transferTransaction = await transferTransactionReadRepository.GetByIdAsync(
            request.WorkspaceId,
            request.TransferTransactionId, cancellationToken);
        if (transferTransaction is null)
        {
            return TransactionErrors.NotFoundById<TransferTransaction>(request.TransferTransactionId);
        }

        return transferTransaction;
    }
}