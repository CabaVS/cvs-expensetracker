using CabaVS.ExpenseTracker.Application.Common.Requests;
using CabaVS.ExpenseTracker.Application.Contracts.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Features.Workspaces.Models;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Workspaces.Queries;

public sealed record GetAllWorkspacesByUserQuery(Guid UserId) : IUserOwningRequest, IRequest<Result<WorkspaceModel[]>>;

internal sealed class GetAllWorkspacesByUserQueryHandler(IReadOnlyWorkspaceRepository readOnlyWorkspaceRepository)
    : IRequestHandler<GetAllWorkspacesByUserQuery, Result<WorkspaceModel[]>>
{
    public async Task<Result<WorkspaceModel[]>> Handle(
        GetAllWorkspacesByUserQuery request,
        CancellationToken cancellationToken)
    {
        WorkspaceModel[] models = await readOnlyWorkspaceRepository.GetAllAsync(request.UserId, cancellationToken);
        return Result<WorkspaceModel[]>.Success(models);
    }
}
