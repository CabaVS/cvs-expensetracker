using CabaVS.ExpenseTracker.Application.Common;
using CabaVS.ExpenseTracker.Application.Contracts.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Features.Workspaces.Models;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Workspaces.Queries;

public sealed record GetAllWorkspacesByUserQuery(Guid UserId) : IUserOwningRequest, IRequest<Result<WorkspaceCollectionItemModel[]>>;

internal sealed class GetAllWorkspacesByUserQueryHandler(IReadOnlyWorkspaceRepository readOnlyWorkspaceRepository)
    : IRequestHandler<GetAllWorkspacesByUserQuery, Result<WorkspaceCollectionItemModel[]>>
{
    public async Task<Result<WorkspaceCollectionItemModel[]>> Handle(
        GetAllWorkspacesByUserQuery request,
        CancellationToken cancellationToken)
    {
        WorkspaceCollectionItemModel[] models = await readOnlyWorkspaceRepository.GetAllAsync(request.UserId, cancellationToken);
        return Result<WorkspaceCollectionItemModel[]>.Success(models);
    }
}
