using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Common.Requests;
using CabaVS.ExpenseTracker.Application.Features.Transactions.Models;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Transactions.Queries;

public sealed record GetAllTransactionsQuery(Guid WorkspaceId) : IWorkspaceBoundRequest, IRequest<Result<TransactionModel[]>>;

internal sealed class GetAllTransactionsHandler(ITransactionQueryRepository transactionQueryRepository) 
    : IRequestHandler<GetAllTransactionsQuery, Result<TransactionModel[]>>
{
    public async Task<Result<TransactionModel[]>> Handle(GetAllTransactionsQuery request, CancellationToken cancellationToken)
    {
        TransactionModel[] allModels = await transactionQueryRepository.GetAllAsync(request.WorkspaceId, cancellationToken);
        return allModels;
    }
}
