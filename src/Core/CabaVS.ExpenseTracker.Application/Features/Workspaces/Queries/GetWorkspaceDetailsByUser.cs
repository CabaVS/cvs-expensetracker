using CabaVS.ExpenseTracker.Application.Common;
using CabaVS.ExpenseTracker.Application.Contracts.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Contracts.Presentation;
using CabaVS.ExpenseTracker.Application.Features.Workspaces.Models;
using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Workspaces.Queries;

public sealed record GetWorkspaceDetailsByUserQuery(Guid UserId, Guid WorkspaceId) 
    : IUserOwningRequest, IRequest<Result<WorkspaceDetailsModel>>;

internal sealed class GetWorkspaceDetailsByUserQueryHandler(
    IReadOnlyWorkspaceRepository readOnlyWorkspaceRepository, 
    ICurrentUserAccessor currentUserAccessor)
    : IRequestHandler<GetWorkspaceDetailsByUserQuery, Result<WorkspaceDetailsModel>>
{
    public async Task<Result<WorkspaceDetailsModel>> Handle(
        GetWorkspaceDetailsByUserQuery request,
        CancellationToken cancellationToken)
    {
        WorkspaceDetailsModel? model = await readOnlyWorkspaceRepository.GetDetailsAsync(
            currentUserAccessor.UserId,
            request.WorkspaceId,
            cancellationToken);
        return model is not null
            ? Result<WorkspaceDetailsModel>.Success(model)
            : Result<WorkspaceDetailsModel>.Failure(
                WorkspaceErrors.WorkspaceNotFoundById(request.WorkspaceId));
    }
}
