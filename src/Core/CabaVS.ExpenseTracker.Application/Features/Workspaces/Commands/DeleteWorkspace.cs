using CabaVS.ExpenseTracker.Application.Abstractions.Persistence;
using CabaVS.ExpenseTracker.Application.Abstractions.Presentation;
using CabaVS.ExpenseTracker.Application.Common.Requests;
using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Workspaces.Commands;

public sealed record DeleteWorkspaceCommand(Guid WorkspaceId) : IRequest<Result>, IAuthenticatedUserRequest;

internal sealed class DeleteWorkspaceCommandHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserAccessor currentUserAccessor) : IRequestHandler<DeleteWorkspaceCommand, Result>
{
    public async Task<Result> Handle(DeleteWorkspaceCommand request, CancellationToken cancellationToken)
    {
        var user = (await currentUserAccessor.GetCurrentUser(cancellationToken))!;
        var repository = unitOfWork.BuildWorkspaceRepository();
        
        var workspace = await repository.GetById(request.WorkspaceId, user.Id, cancellationToken);
        if (workspace is null) return WorkspaceErrors.NotFoundById(request.WorkspaceId);

        var isAdminOverWorkspace = await repository.IsAdmin(request.WorkspaceId, user.Id, cancellationToken);
        if (!isAdminOverWorkspace) return WorkspaceErrors.AdminRightsRequired(request.WorkspaceId);
        
        await repository.Delete(workspace, cancellationToken);
        await unitOfWork.SaveChanges(cancellationToken);
        
        return Result.Success();
    }
}