using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Abstractions.Presentation;
using CabaVS.ExpenseTracker.Application.Abstractions.Presentation.Models;
using CabaVS.ExpenseTracker.Application.Common.Requests;
using CabaVS.ExpenseTracker.Application.Features.Workspaces.Models;
using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Workspaces.Queries;

public sealed record GetWorkspaceByIdQuery(Guid Id) : IRequest<Result<WorkspaceModel>>, IAuthenticatedUserRequest;

internal sealed class GetWorkspaceByIdQueryHandler(
    ICurrentUserAccessor currentUserAccessor,
    IWorkspaceReadRepository workspaceReadRepository) : IRequestHandler<GetWorkspaceByIdQuery, Result<WorkspaceModel>>
{
    public async Task<Result<WorkspaceModel>> Handle(GetWorkspaceByIdQuery request, CancellationToken cancellationToken)
    {
        AuthenticatedUserModel currentUser = (await currentUserAccessor.GetCurrentUser(cancellationToken))!;
        
        WorkspaceModel? workspaceModel = await workspaceReadRepository.GetWorkspaceById(request.Id, currentUser.Id, cancellationToken);
        return workspaceModel is null ? WorkspaceErrors.NotFoundById(request.Id) : workspaceModel;
    }
}
