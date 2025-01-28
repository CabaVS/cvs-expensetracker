using CabaVS.ExpenseTracker.Application.Abstractions.Presentation;
using CabaVS.ExpenseTracker.Application.Common.Requests;
using CabaVS.ExpenseTracker.Application.Models;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Common.Behaviors;

internal sealed class AuthenticatedUserRequestBehavior<TRequest, TResponse>(
    ICurrentUserAccessor currentUserAccessor) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IAuthenticatedUserRequest
    where TResponse : Result
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        AuthenticatedUserModel? currentUser = await currentUserAccessor.GetCurrentUserAsync(cancellationToken);
        return currentUser is not null
            ? await next()
            : throw new InvalidOperationException();
    }
}
