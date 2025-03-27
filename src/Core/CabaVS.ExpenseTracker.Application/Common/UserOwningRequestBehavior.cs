using CabaVS.ExpenseTracker.Application.Contracts.Presentation;
using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Common;

internal sealed class UserOwningRequestBehavior<TRequest, TResponse>(ICurrentUserAccessor currentUserAccessor) 
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IUserOwningRequest
    where TResponse : Result
{
    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        Guid currentUserId = currentUserAccessor.UserId;
        return request.UserId == currentUserId
            ? await next()
            : FailedResultFactory.Create<TResponse>(
                UserErrors.UserHasNoAccess(currentUserId));
    }
}
