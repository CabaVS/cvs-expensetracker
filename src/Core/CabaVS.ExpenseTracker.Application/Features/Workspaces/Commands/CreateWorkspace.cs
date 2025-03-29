using CabaVS.ExpenseTracker.Application.Contracts.Persistence;
using CabaVS.ExpenseTracker.Application.Contracts.Presentation;
using CabaVS.ExpenseTracker.Domain.Entities;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Workspaces.Commands;

public sealed record CreateWorkspaceCommand(string WorkspaceName) : IRequest<Result<Guid>>;

internal sealed class CreateWorkspaceCommandHandler(IUnitOfWork unitOfWork, ICurrentUserAccessor currentUserAccessor) 
    : IRequestHandler<CreateWorkspaceCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateWorkspaceCommand request, CancellationToken cancellationToken)
    {
        User owner = await unitOfWork.UserRepository.GetAsync(
            currentUserAccessor.UserId,
            cancellationToken) ?? throw new InvalidOperationException("User not found.");

        Result<Workspace> workspace = Workspace.CreateNew(
            request.WorkspaceName,
            owner);
        if (workspace.IsFailure)
        {
            return Result<Guid>.Failure(workspace.Error);
        }
            
        Guid createdId = await unitOfWork.WorkspaceRepository.CreateAsync(workspace.Value, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result<Guid>.Success(createdId);
    }
}
