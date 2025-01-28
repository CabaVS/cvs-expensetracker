using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Common.Requests;
using CabaVS.ExpenseTracker.Application.Features.Transactions.Models;
using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Transactions.Queries;

public sealed record GetTransactionByIdQuery(Guid TransactionId, Guid WorkspaceId) : IWorkspaceBoundRequest, IRequest<Result<TransactionModel>>;

internal sealed class GetTransactionByIdQueryHandler(
    ITransactionQueryRepository transactionQueryRepository) : IRequestHandler<GetTransactionByIdQuery, Result<TransactionModel>>
{
    public async Task<Result<TransactionModel>> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
    {
        TransactionModel? transactionModel = await transactionQueryRepository.GetByIdAsync(request.WorkspaceId, request.TransactionId, cancellationToken);
        return transactionModel is not null ? transactionModel : TransactionErrors.NotFoundById(request.TransactionId);
    }
}
