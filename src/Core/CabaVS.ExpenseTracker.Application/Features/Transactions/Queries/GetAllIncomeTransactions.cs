using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Common.Abstractions;
using CabaVS.ExpenseTracker.Application.Features.Transactions.Models;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Transactions.Queries;

public sealed record GetAllIncomeTransactionsQuery(
    Guid WorkspaceId) : IRequest<Result<IncomeTransactionModel[]>>, IWorkspaceBoundedRequest;

internal sealed class GetAllIncomeTransactionsQueryHandler(
    IIncomeTransactionReadRepository incomeTransactionReadRepository) : IRequestHandler<GetAllIncomeTransactionsQuery, Result<IncomeTransactionModel[]>>
{
    public async Task<Result<IncomeTransactionModel[]>> Handle(GetAllIncomeTransactionsQuery request, CancellationToken cancellationToken)
    {
        var models = await incomeTransactionReadRepository.GetAll(
            request.WorkspaceId, cancellationToken);
        return models;
    }
}