using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Abstractions.Presentation;
using CabaVS.ExpenseTracker.Application.Common.Requests;
using CabaVS.ExpenseTracker.Application.Models;
using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Common.Behaviors;

internal sealed class WorkspaceBoundRequestBehavior<TRequest, TResponse>(
    ICurrentUserAccessor currentUserAccessor,
    IWorkspaceQueryRepository workspaceQueryRepository) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IWorkspaceBoundRequest
    where TResponse : Result
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        AuthenticatedUserModel currentUser = await currentUserAccessor.GetCurrentUserAsync(cancellationToken) 
                                             ?? throw new InvalidOperationException();
        
        var isMemberOfWorkspace = await workspaceQueryRepository.IsMemberOfWorkspaceAsync(currentUser.Id, request.WorkspaceId, cancellationToken);
        return isMemberOfWorkspace
            ? await next()
            : FailedResultFactory.Create<TResponse>(
                WorkspaceErrors.NotFoundById(request.WorkspaceId));
    }
}
