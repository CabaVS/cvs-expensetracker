using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Abstractions.Presentation;
using CabaVS.ExpenseTracker.Application.Common.Abstractions;
using CabaVS.ExpenseTracker.Application.Common.Errors;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Common.Behaviors;

internal sealed class WorkspaceAdminBoundedRequestBehavior<TRequest, TResponse>(
    ICurrentUserAccessor userAccessor,
    IUserReadRepository userReadRepository) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IWorkspaceAdminBoundedRequest
    where TResponse : Result
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var userId = await userAccessor.GetId(cancellationToken);

        var hasAccess = await userReadRepository.HasAccessToWorkspace(userId, request.WorkspaceId, cancellationToken);
        if (!hasAccess)
            return FailedResultFactory.Create<TResponse>(
                WorkspaceAccessErrors.NoAccess(request.WorkspaceId));

        var hasAdminAccess = await userReadRepository.HasAdminAccessToWorkspace(userId, request.WorkspaceId, cancellationToken);
        return hasAdminAccess
            ? await next()
            : FailedResultFactory.Create<TResponse>(
                WorkspaceAccessErrors.NotAdmin(request.WorkspaceId));
    }
}