using CabaVS.ExpenseTracker.Application.Common.Requests;
using CabaVS.ExpenseTracker.Application.Contracts.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Features.Balances.Models;
using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Balances.Queries;

public sealed record GetBalanceByIdQuery(Guid WorkspaceId, Guid BalanceId) : IWorkspaceBoundedRequest, IRequest<Result<BalanceModel>>;

internal sealed class GetBalanceByIdQueryHandler(IReadOnlyBalanceRepository readOnlyBalanceRepository)
    : IRequestHandler<GetBalanceByIdQuery, Result<BalanceModel>>
{
    public async Task<Result<BalanceModel>> Handle(
        GetBalanceByIdQuery request,
        CancellationToken cancellationToken)
    {
        BalanceModel? model = await readOnlyBalanceRepository.GetDetailsAsync(
            request.WorkspaceId,
            request.BalanceId,
            cancellationToken);
        return model is not null
            ? model
            : BalanceErrors.BalanceNotFoundById(request.BalanceId);
    }
}
