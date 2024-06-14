using CabaVS.ExpenseTracker.Application.Abstractions.Persistence;
using CabaVS.ExpenseTracker.Application.Abstractions.Presentation;
using CabaVS.ExpenseTracker.Application.Common.Abstractions;
using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Workspaces.Commands;

public sealed record DeleteWorkspaceCommand(Guid WorkspaceId) : IRequest<Result>, IWorkspaceAdminBoundedRequest;

internal sealed class DeleteWorkspaceCommandHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserAccessor currentUserAccessor) : IRequestHandler<DeleteWorkspaceCommand, Result>
{
    public async Task<Result> Handle(DeleteWorkspaceCommand request, CancellationToken cancellationToken)
    {
        var userId = await currentUserAccessor.GetId(cancellationToken);

        var workspace = await unitOfWork.WorkspaceWriteRepository.GetById(request.WorkspaceId, userId, cancellationToken);
        if (workspace is null) return WorkspaceErrors.NotFoundById(request.WorkspaceId);

        await unitOfWork.WorkspaceWriteRepository.Delete(workspace, userId, cancellationToken);
        await unitOfWork.SaveChanges(cancellationToken);
        
        return Result.Success();
    }
}