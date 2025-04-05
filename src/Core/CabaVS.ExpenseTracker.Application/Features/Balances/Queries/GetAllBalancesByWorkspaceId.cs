using CabaVS.ExpenseTracker.Application.Common.Requests;
using CabaVS.ExpenseTracker.Application.Contracts.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Features.Balances.Models;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Balances.Queries;

public sealed record GetAllBalancesByWorkspaceIdQuery(Guid WorkspaceId) : IWorkspaceBoundedRequest, IRequest<Result<BalanceModel[]>>;

internal sealed class GetAllBalancesByWorkspaceIdQueryHandler(IReadOnlyBalanceRepository readOnlyBalanceRepository)
    : IRequestHandler<GetAllBalancesByWorkspaceIdQuery, Result<BalanceModel[]>>
{
    public async Task<Result<BalanceModel[]>> Handle(
        GetAllBalancesByWorkspaceIdQuery request,
        CancellationToken cancellationToken)
    {
        BalanceModel[] models = await readOnlyBalanceRepository.GetAllAsync(request.WorkspaceId, cancellationToken);
        return Result<BalanceModel[]>.Success(models);
    }
}
