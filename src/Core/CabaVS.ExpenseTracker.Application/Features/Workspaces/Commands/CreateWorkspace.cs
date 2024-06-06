using CabaVS.ExpenseTracker.Application.Abstractions.Persistence;
using CabaVS.ExpenseTracker.Application.Abstractions.Presentation;
using CabaVS.ExpenseTracker.Domain.Entities;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Workspaces.Commands;

public sealed record CreateWorkspaceCommand(string Name) : IRequest<Result<Guid>>;

internal sealed class CreateWorkspaceCommandHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserAccessor currentUserAccessor) : IRequestHandler<CreateWorkspaceCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateWorkspaceCommand request, CancellationToken cancellationToken)
    {
        var workspaceResult = Workspace.Create(Guid.NewGuid(), request.Name);
        if (workspaceResult.IsFailure) return workspaceResult.Error;

        var id = await unitOfWork.WorkspaceWriteRepository.Create(
            workspaceResult.Value,
            (await currentUserAccessor.Get(cancellationToken)).Id,
            true,
            cancellationToken);
        await unitOfWork.SaveChanges(cancellationToken);

        return id;
    }
}