using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Common.Abstractions;
using CabaVS.ExpenseTracker.Application.Features.Balances.Models;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Balances.Queries;

public sealed record GetAllBalancesQuery(Guid WorkspaceId) : IRequest<Result<BalanceModel[]>>, IWorkspaceBoundedRequest;

internal sealed class GetAllBalancesQueryHandler(
    IBalanceReadRepository balanceReadRepository) : IRequestHandler<GetAllBalancesQuery, Result<BalanceModel[]>>
{
    public async Task<Result<BalanceModel[]>> Handle(GetAllBalancesQuery request, CancellationToken cancellationToken)
    {
        var models = await balanceReadRepository.GetAll(request.WorkspaceId, cancellationToken);
        return models;
    }
}