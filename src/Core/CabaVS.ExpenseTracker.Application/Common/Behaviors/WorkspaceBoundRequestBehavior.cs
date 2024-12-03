using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Abstractions.Presentation;
using CabaVS.ExpenseTracker.Application.Common.Requests;
using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Common.Behaviors;

internal sealed class WorkspaceBoundRequestBehavior<TRequest, TResponse>(
    ICurrentUserAccessor currentUserAccessor,
    IWorkspaceReadRepository workspaceReadRepository) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IWorkspaceBoundRequest
    where TResponse : Result
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var currentUser = await currentUserAccessor.GetCurrentUser(cancellationToken) 
                          ?? throw new InvalidOperationException("Current user is null.");
        
        var isMemberOfWorkspace = await workspaceReadRepository.IsMemberOfWorkspace(request.WorkspaceId, currentUser.Id, cancellationToken);
        return isMemberOfWorkspace
            ? await next()
            : FailedResultFactory.Create<TResponse>(
                WorkspaceErrors.NotFoundById(request.WorkspaceId));
    }
}