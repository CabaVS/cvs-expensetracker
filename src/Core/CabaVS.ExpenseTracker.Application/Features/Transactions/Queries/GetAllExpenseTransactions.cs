using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Common.Abstractions;
using CabaVS.ExpenseTracker.Application.Features.Transactions.Models;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Transactions.Queries;

public sealed record GetAllExpenseTransactionsQuery(
    Guid WorkspaceId) : IRequest<Result<ExpenseTransactionModel[]>>, IWorkspaceBoundedRequest;

internal sealed class GetAllExpenseTransactionsQueryHandler(
    IExpenseTransactionReadRepository expenseTransactionReadRepository) : IRequestHandler<GetAllExpenseTransactionsQuery, Result<ExpenseTransactionModel[]>>
{
    public async Task<Result<ExpenseTransactionModel[]>> Handle(GetAllExpenseTransactionsQuery request, CancellationToken cancellationToken)
    {
        var models = await expenseTransactionReadRepository.GetAll(
            request.WorkspaceId, cancellationToken);
        return models;
    }
}