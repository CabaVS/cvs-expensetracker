using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Abstractions.Presentation;
using CabaVS.ExpenseTracker.Application.Common.Abstractions;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Common.Behaviors;

internal sealed class WorkspaceBoundedRequestBehavior<TRequest, TResponse>(
    ICurrentUserAccessor userAccessor,
    IUserReadRepository userReadRepository) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IWorkspaceBoundedRequest
    where TResponse : Result
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var userId = await userAccessor.GetId(cancellationToken);

        var hasAccess = await userReadRepository.HasAccessToWorkspace(userId, request.WorkspaceId, cancellationToken);
        return hasAccess
            ? await next()
            : FailedResultFactory.Create<TResponse>(
                new Error("Workspace.AccessDenied", "Current User has no access over requested Workspace."));
    }
}