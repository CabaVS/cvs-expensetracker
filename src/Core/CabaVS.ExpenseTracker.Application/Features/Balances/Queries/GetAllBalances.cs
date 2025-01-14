using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Common.Requests;
using CabaVS.ExpenseTracker.Application.Features.Balances.Models;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Balances.Queries;

public sealed record GetAllBalancesQuery(Guid WorkspaceId) : IWorkspaceBoundRequest, IRequest<Result<BalanceModel[]>>;

internal sealed class GetAllBalancesHandler(IBalanceReadRepository balanceReadRepository) 
    : IRequestHandler<GetAllBalancesQuery, Result<BalanceModel[]>>
{
    public async Task<Result<BalanceModel[]>> Handle(GetAllBalancesQuery request, CancellationToken cancellationToken)
    {
        BalanceModel[] allModels = await balanceReadRepository.GetBalances(request.WorkspaceId, cancellationToken);
        return allModels;
    }
}
