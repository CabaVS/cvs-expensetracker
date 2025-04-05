using CabaVS.ExpenseTracker.Application.Common.Requests;
using CabaVS.ExpenseTracker.Application.Contracts.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Contracts.Presentation;
using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Common.Behaviors;

internal sealed class WorkspaceBoundedRequestBehavior<TRequest, TResponse>(
    ICurrentUserAccessor currentUserAccessor,
    IReadOnlyWorkspaceRepository readOnlyWorkspaceRepository) 
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IWorkspaceBoundedRequest
    where TResponse : Result
{
    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        Guid currentUserId = currentUserAccessor.UserId!.Value;
        return await readOnlyWorkspaceRepository.UserIsMemberOfWorkspaceAsync(currentUserId, request.WorkspaceId, cancellationToken)
            ? await next()
            : FailedResultFactory.Create<TResponse>(
                UserErrors.UserHasNoAccess(currentUserId));
    }
}
