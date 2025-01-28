using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Abstractions.Presentation;
using CabaVS.ExpenseTracker.Application.Common.Requests;
using CabaVS.ExpenseTracker.Application.Features.Workspaces.Models;
using CabaVS.ExpenseTracker.Application.Models;
using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Workspaces.Queries;

public sealed record GetWorkspaceByIdQuery(Guid WorkspaceId) : IRequest<Result<WorkspaceModel>>, IAuthenticatedUserRequest;

internal sealed class GetWorkspaceByIdQueryHandler(
    ICurrentUserAccessor currentUserAccessor,
    IWorkspaceQueryRepository workspaceQueryRepository) : IRequestHandler<GetWorkspaceByIdQuery, Result<WorkspaceModel>>
{
    public async Task<Result<WorkspaceModel>> Handle(GetWorkspaceByIdQuery request, CancellationToken cancellationToken)
    {
        AuthenticatedUserModel currentUser = (await currentUserAccessor.GetCurrentUserAsync(cancellationToken))!;
        
        WorkspaceModel? workspaceModel = await workspaceQueryRepository.GetByIdAsync(currentUser.Id, request.WorkspaceId, cancellationToken);
        return workspaceModel is not null ? workspaceModel : WorkspaceErrors.NotFoundById(request.WorkspaceId);
    }
}
