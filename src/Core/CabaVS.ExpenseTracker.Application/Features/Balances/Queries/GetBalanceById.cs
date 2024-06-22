using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Common.Abstractions;
using CabaVS.ExpenseTracker.Application.Features.Balances.Models;
using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Balances.Queries;

public sealed record GetBalanceByIdQuery(Guid WorkspaceId, Guid BalanceId)
    : IRequest<Result<BalanceModel>>, IWorkspaceBoundedRequest;

internal sealed class GetBalanceByIdQueryHandler(IBalanceReadRepository balanceReadRepository)
    : IRequestHandler<GetBalanceByIdQuery, Result<BalanceModel>>
{
    public async Task<Result<BalanceModel>> Handle(GetBalanceByIdQuery request, CancellationToken cancellationToken)
    {
        var model = await balanceReadRepository.GetById(request.BalanceId, request.WorkspaceId, cancellationToken);
        if (model is null) return BalanceErrors.NotFoundById(request.BalanceId);

        return model;
    }
}