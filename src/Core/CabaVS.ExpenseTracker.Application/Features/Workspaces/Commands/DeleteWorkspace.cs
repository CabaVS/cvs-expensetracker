using CabaVS.ExpenseTracker.Application.Abstractions.Persistence;
using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Abstractions.Presentation;
using CabaVS.ExpenseTracker.Application.Common.Requests;
using CabaVS.ExpenseTracker.Application.Models;
using CabaVS.ExpenseTracker.Domain.Entities;
using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Workspaces.Commands;

public sealed record DeleteWorkspaceCommand(Guid WorkspaceId) : IRequest<Result>, IAuthenticatedUserRequest;

internal sealed class DeleteWorkspaceCommandHandler(
    IUnitOfWork unitOfWork,
    IWorkspaceQueryRepository workspaceQueryRepository,
    ICurrentUserAccessor currentUserAccessor) : IRequestHandler<DeleteWorkspaceCommand, Result>
{
    public async Task<Result> Handle(DeleteWorkspaceCommand request, CancellationToken cancellationToken)
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

        await unitOfWork.WorkspaceRepository.DeleteAsync(workspace, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}
