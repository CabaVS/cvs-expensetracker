using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Common.Requests;
using CabaVS.ExpenseTracker.Application.Features.Balances.Models;
using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Balances.Queries;

public sealed record GetBalanceByIdQuery(Guid BalanceId, Guid WorkspaceId) : IWorkspaceBoundRequest, IRequest<Result<BalanceModel>>;

internal sealed class GetBalanceByIdQueryHandler(
    IBalanceQueryRepository balanceQueryRepository) : IRequestHandler<GetBalanceByIdQuery, Result<BalanceModel>>
{
    public async Task<Result<BalanceModel>> Handle(GetBalanceByIdQuery request, CancellationToken cancellationToken)
    {
        BalanceModel? balanceModel = await balanceQueryRepository.GetByIdAsync(request.WorkspaceId, request.BalanceId, cancellationToken);
        return balanceModel is not null ? balanceModel : BalanceErrors.NotFoundById(request.BalanceId);
    }
}
