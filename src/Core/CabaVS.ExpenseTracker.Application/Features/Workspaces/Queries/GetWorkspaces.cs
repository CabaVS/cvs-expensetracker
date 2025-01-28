using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Abstractions.Presentation;
using CabaVS.ExpenseTracker.Application.Common.Requests;
using CabaVS.ExpenseTracker.Application.Features.Workspaces.Models;
using CabaVS.ExpenseTracker.Application.Models;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Workspaces.Queries;

public sealed record GetWorkspacesQuery : IRequest<Result<WorkspaceModel[]>>, IAuthenticatedUserRequest;

internal sealed class GetWorkspacesQueryHandler(
    ICurrentUserAccessor currentUserAccessor,
    IWorkspaceQueryRepository workspaceQueryRepository) : IRequestHandler<GetWorkspacesQuery, Result<WorkspaceModel[]>>
{
    public async Task<Result<WorkspaceModel[]>> Handle(GetWorkspacesQuery request, CancellationToken cancellationToken)
    {
        AuthenticatedUserModel currentUser = (await currentUserAccessor.GetCurrentUserAsync(cancellationToken))!;
        
        WorkspaceModel[] workspaceModels = await workspaceQueryRepository.GetAllAsync(currentUser.Id, cancellationToken);
        return workspaceModels;
    }
}
