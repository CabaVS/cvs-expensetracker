using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Common.Abstractions;
using CabaVS.ExpenseTracker.Application.Features.Transactions.Models;
using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Transactions.Queries;

public sealed record GetExpenseTransactionByIdQuery(Guid ExpenseTransactionId, Guid WorkspaceId)
    : IRequest<Result<ExpenseTransactionModel>>, IWorkspaceBoundedRequest;

internal sealed class GetExpenseTransactionByIdQueryHandler(IExpenseTransactionReadRepository expenseTransactionReadRepository)
    : IRequestHandler<GetExpenseTransactionByIdQuery, Result<ExpenseTransactionModel>>
{
    public async Task<Result<ExpenseTransactionModel>> Handle(GetExpenseTransactionByIdQuery request, CancellationToken cancellationToken)
    {
        var model = await expenseTransactionReadRepository.GetById(
            request.ExpenseTransactionId, request.WorkspaceId, cancellationToken);
        return model is not null 
            ? model
            : TransactionErrors.NotFoundById(request.ExpenseTransactionId);
    }
}