using CabaVS.ExpenseTracker.Application.Common.Requests;
using CabaVS.ExpenseTracker.Application.Contracts.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Features.Transactions.Models;
using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Transactions.Queries;

public sealed record GetSingleTransactionByIdQuery(Guid WorkspaceId, Guid TransactionId)
    : IWorkspaceBoundedRequest, IRequest<Result<TransactionModel>>;

internal sealed class GetSingleTransactionByIdQueryHandler(IReadOnlyTransactionRepository readOnlyTransactionRepository)
    : IRequestHandler<GetSingleTransactionByIdQuery, Result<TransactionModel>>
{
    public async Task<Result<TransactionModel>> Handle(GetSingleTransactionByIdQuery request, CancellationToken cancellationToken)
    {
        TransactionModel? transactionModel = await readOnlyTransactionRepository.GetSingleByIdAsync(
            request.WorkspaceId, request.TransactionId, cancellationToken);
        return transactionModel is not null
            ? transactionModel
            : TransactionErrors.NotFoundById(request.TransactionId);
    }
}
