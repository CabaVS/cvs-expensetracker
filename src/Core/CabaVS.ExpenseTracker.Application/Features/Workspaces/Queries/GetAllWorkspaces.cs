using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Abstractions.Presentation;
using CabaVS.ExpenseTracker.Application.Features.Workspaces.Models;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Workspaces.Queries;

public sealed record GetAllWorkspacesQuery : IRequest<Result<WorkspaceModel[]>>;

internal sealed class GetAllWorkspacesQueryHandler(
    IWorkspaceReadRepository workspaceReadRepository,
    ICurrentUserAccessor currentUserAccessor) : IRequestHandler<GetAllWorkspacesQuery, Result<WorkspaceModel[]>>
{
    public async Task<Result<WorkspaceModel[]>> Handle(GetAllWorkspacesQuery request, CancellationToken cancellationToken)
    {
        var userId = await currentUserAccessor.GetId(cancellationToken);
        
        var models = await workspaceReadRepository.GetAll(userId, cancellationToken);
        return models;
    }
}