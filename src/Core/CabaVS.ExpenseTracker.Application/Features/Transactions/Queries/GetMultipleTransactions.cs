using CabaVS.ExpenseTracker.Application.Common.Requests;
using CabaVS.ExpenseTracker.Application.Contracts.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Features.Transactions.Models;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Transactions.Queries;

public sealed record GetMultipleTransactionsQuery(Guid WorkspaceId, DateOnly From, DateOnly To) 
    : IWorkspaceBoundedRequest, IRequest<Result<TransactionModel[]>>;

internal sealed class GetMultipleTransactionsQueryHandler(IReadOnlyTransactionRepository readOnlyTransactionRepository) 
    : IRequestHandler<GetMultipleTransactionsQuery, Result<TransactionModel[]>>
{
    public async Task<Result<TransactionModel[]>> Handle(
        GetMultipleTransactionsQuery request,
        CancellationToken cancellationToken)
    {
        TransactionModel[] transactionModels = await readOnlyTransactionRepository.GetMultipleAsync(
            request.WorkspaceId, request.From, request.To, cancellationToken);
        return transactionModels;
    }
}
