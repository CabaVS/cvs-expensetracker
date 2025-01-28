using CabaVS.ExpenseTracker.Application.Abstractions.Persistence;
using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Abstractions.Presentation;
using CabaVS.ExpenseTracker.Application.Common.Requests;
using CabaVS.ExpenseTracker.Application.Models;
using CabaVS.ExpenseTracker.Domain.Entities;
using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Domain.ValueObjects;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Workspaces.Commands;

public sealed record UpdateWorkspaceCommand(Guid WorkspaceId, string Name) : IRequest<Result>, IWorkspaceBoundRequest;

internal sealed class UpdateWorkspaceCommandHandler(
    IUnitOfWork unitOfWork,
    IWorkspaceQueryRepository workspaceQueryRepository,
    ICurrentUserAccessor currentUserAccessor) : IRequestHandler<UpdateWorkspaceCommand, Result>
{
    public async Task<Result> Handle(UpdateWorkspaceCommand request, CancellationToken cancellationToken)
    {
        AuthenticatedUserModel currentUser = (await currentUserAccessor.GetCurrentUserAsync(cancellationToken))!;
        
        Workspace? workspace = await unitOfWork.WorkspaceRepository.GetByIdAsync(request.WorkspaceId, cancellationToken);
        if (workspace is null)
        {
            return WorkspaceErrors.NotFoundById(request.WorkspaceId);
        }
        
        var isAdminOverWorkspace = await workspaceQueryRepository.IsAdminOfWorkspaceAsync(currentUser.Id, request.WorkspaceId, cancellationToken);
        if (!isAdminOverWorkspace)
        {
            return WorkspaceErrors.UserIsNotAnAdminOverWorkspace();
        }

        Result<WorkspaceName> nameCreationResult = WorkspaceName.Create(request.Name);
        if (nameCreationResult.IsFailure)
        {
            return nameCreationResult.Error;
        }
        
        workspace.Name = nameCreationResult.Value;

        await unitOfWork.WorkspaceRepository.UpdateAsync(workspace, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
