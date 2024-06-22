using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Abstractions.Presentation;
using CabaVS.ExpenseTracker.Application.Common.Abstractions;
using CabaVS.ExpenseTracker.Application.Features.Workspaces.Models;
using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Workspaces.Queries;

public sealed record GetWorkspaceByIdQuery(Guid WorkspaceId)
    : IRequest<Result<WorkspaceModel>>, IWorkspaceBoundedRequest;

internal sealed class GetWorkspaceByIdQueryHandler(
    IWorkspaceReadRepository workspaceReadRepository,
    ICurrentUserAccessor currentUserAccessor)
    : IRequestHandler<GetWorkspaceByIdQuery, Result<WorkspaceModel>>
{
    public async Task<Result<WorkspaceModel>> Handle(GetWorkspaceByIdQuery request, CancellationToken cancellationToken)
    {
        var userId = await currentUserAccessor.GetId(cancellationToken);

        var model = await workspaceReadRepository.GetById(request.WorkspaceId, userId, cancellationToken);
        return model is not null 
            ? model 
            : WorkspaceErrors.NotFoundById(request.WorkspaceId);
    }
}