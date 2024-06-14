using CabaVS.ExpenseTracker.Application.Abstractions.Persistence;
using CabaVS.ExpenseTracker.Application.Abstractions.Presentation;
using CabaVS.ExpenseTracker.Application.Common.Abstractions;
using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Domain.ValueObjects;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Workspaces.Commands;

public sealed record UpdateWorkspaceCommand(Guid WorkspaceId, string Name) : IRequest<Result>, IWorkspaceAdminBoundedRequest;

internal sealed class UpdateWorkspaceCommandHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserAccessor userAccessor) : IRequestHandler<UpdateWorkspaceCommand, Result>
{
    public async Task<Result> Handle(UpdateWorkspaceCommand request, CancellationToken cancellationToken)
    {
        var workspaceNameResult = WorkspaceName.Create(request.Name);
        if (workspaceNameResult.IsFailure) return workspaceNameResult.Error;

        var userId = await userAccessor.GetId(cancellationToken);
        
        var workspace = await unitOfWork.WorkspaceWriteRepository.GetById(request.WorkspaceId, userId, cancellationToken);
        if (workspace is null) return WorkspaceErrors.NotFoundById(request.WorkspaceId);

        workspace.Name = workspaceNameResult.Value;

        await unitOfWork.WorkspaceWriteRepository.Update(workspace, userId, cancellationToken);
        await unitOfWork.SaveChanges(cancellationToken);
        
        return Result.Success();
    }
}