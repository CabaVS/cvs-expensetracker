using CabaVS.ExpenseTracker.Application.Abstractions.Persistence;
using CabaVS.ExpenseTracker.Application.Abstractions.Presentation;
using CabaVS.ExpenseTracker.Application.Common.Requests;
using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Domain.ValueObjects;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Workspaces.Commands;

public sealed record UpdateWorkspaceCommand(Guid WorkspaceId, string Name) : IRequest<Result>, IAuthenticatedUserRequest;

internal sealed class UpdateWorkspaceCommandHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserAccessor currentUserAccessor) : IRequestHandler<UpdateWorkspaceCommand, Result>
{
    public async Task<Result> Handle(UpdateWorkspaceCommand request, CancellationToken cancellationToken)
    {
        var user = (await currentUserAccessor.GetCurrentUser(cancellationToken))!;
        var repository = unitOfWork.BuildWorkspaceRepository();
        
        var workspace = await repository.GetById(request.WorkspaceId, user.Id, cancellationToken);
        if (workspace is null) return WorkspaceErrors.NotFoundById(request.WorkspaceId);
        
        if (!await repository.IsAdmin(request.WorkspaceId, user.Id, cancellationToken))
            return WorkspaceErrors.AdminRightsRequired(request.WorkspaceId);
        
        var updateNameResult = WorkspaceName.Create(request.Name);
        if (updateNameResult.IsFailure) return updateNameResult.Error;
        workspace.Name = updateNameResult.Value;
        
        await repository.Update(workspace, cancellationToken);
        await unitOfWork.SaveChanges(cancellationToken);

        return Result.Success();
    }
}