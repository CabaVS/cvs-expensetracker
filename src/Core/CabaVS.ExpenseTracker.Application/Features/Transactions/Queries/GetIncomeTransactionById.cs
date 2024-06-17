using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Common.Abstractions;
using CabaVS.ExpenseTracker.Application.Features.Transactions.Models;
using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Transactions.Queries;

public sealed record GetIncomeTransactionByIdQuery(
    Guid IncomeTransactionId, Guid WorkspaceId) : IRequest<Result<IncomeTransactionModel>>, IWorkspaceBoundedRequest;

internal sealed class GetIncomeTransactionByIdQueryHandler(
    IIncomeTransactionReadRepository incomeTransactionReadRepository) : IRequestHandler<GetIncomeTransactionByIdQuery, Result<IncomeTransactionModel>>
{
    public async Task<Result<IncomeTransactionModel>> Handle(GetIncomeTransactionByIdQuery request, CancellationToken cancellationToken)
    {
        var model = await incomeTransactionReadRepository.GetById(
            request.IncomeTransactionId, request.WorkspaceId, cancellationToken);
        return model is not null 
            ? model
            : TransactionErrors.NotFoundById(request.IncomeTransactionId);
    }
}