using CabaVS.ExpenseTracker.Application.Abstractions.Persistence;
using CabaVS.ExpenseTracker.Application.Abstractions.Presentation;
using CabaVS.ExpenseTracker.Application.Common.Requests;
using CabaVS.ExpenseTracker.Application.Models;
using CabaVS.ExpenseTracker.Domain.Entities;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Workspaces.Commands;

public sealed record CreateWorkspaceCommand(string Name) : IRequest<Result<Guid>>, IAuthenticatedUserRequest;

internal sealed class CreateWorkspaceCommandHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserAccessor currentUserAccessor) : IRequestHandler<CreateWorkspaceCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateWorkspaceCommand request, CancellationToken cancellationToken)
    {
        AuthenticatedUserModel currentUser = (await currentUserAccessor.GetCurrentUserAsync(cancellationToken))!;
        
        Result<Workspace> workspaceCreationResult = Workspace.Create(request.Name);
        if (workspaceCreationResult.IsFailure)
        {
            return workspaceCreationResult.Error;
        }
        
        Guid workspaceId = await unitOfWork.WorkspaceRepository.CreateAsync(workspaceCreationResult.Value, cancellationToken);
        await unitOfWork.WorkspaceRepository.AddUserAsync(workspaceId, currentUser.Id, true, cancellationToken);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return workspaceId;
    }
}
