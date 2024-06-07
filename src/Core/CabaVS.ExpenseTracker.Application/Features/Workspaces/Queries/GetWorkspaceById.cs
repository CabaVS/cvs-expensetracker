using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Abstractions.Presentation;
using CabaVS.ExpenseTracker.Application.Features.Workspaces.Models;
using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Workspaces.Queries;

public sealed record GetWorkspaceByIdQuery(Guid Id) : IRequest<Result<WorkspaceModel>>;

internal sealed class GetWorkspaceByIdQueryHandler(
    IWorkspaceReadRepository workspaceReadRepository,
    ICurrentUserAccessor currentUserAccessor) : IRequestHandler<GetWorkspaceByIdQuery, Result<WorkspaceModel>>
{
    public async Task<Result<WorkspaceModel>> Handle(GetWorkspaceByIdQuery request, CancellationToken cancellationToken)
    {
        var userId = (await currentUserAccessor.Get(cancellationToken)).Id;

        var model = await workspaceReadRepository.GetById(request.Id, userId, cancellationToken);
        if (model is null) return WorkspaceErrors.NotFoundById(request.Id);

        return model;
    }
}