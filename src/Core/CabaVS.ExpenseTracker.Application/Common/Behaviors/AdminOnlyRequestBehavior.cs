using CabaVS.ExpenseTracker.Application.Abstractions.Presentation;
using CabaVS.ExpenseTracker.Application.Common.Abstractions;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Common.Behaviors;

internal sealed class AdminOnlyRequestBehavior<TRequest, TResponse>(
    ICurrentUserAccessor userAccessor) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IAdminOnlyRequest
    where TResponse : Result
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var currentUser = await userAccessor.Get(cancellationToken);
        return currentUser.IsAdmin
            ? await next()
            : FailedResultFactory.Create<TResponse>(
                new Error("Admin.PermissionsRequired", "Admin permissions required for this operation."));
    }
}