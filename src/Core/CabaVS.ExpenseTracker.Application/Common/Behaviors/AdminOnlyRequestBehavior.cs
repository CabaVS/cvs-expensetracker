using CabaVS.ExpenseTracker.Application.Abstractions.Presentation;
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
            : CreateFailedResponse();
    }

    private static TResponse CreateFailedResponse()
    {
        var error = new Error("Admin.PermissionsRequired", "Admin permissions required for this operation.");
        
        if (typeof(TResponse) == typeof(Result))
        {
            return (TResponse)Result.Failure(error);
        }

        var result = typeof(Result<>)
            .GetGenericTypeDefinition()
            .MakeGenericType(typeof(TResponse).GetGenericArguments()[0])
            .GetMethod(nameof(Result.Failure))!
            .Invoke(null, [error])!;
        return (TResponse)result;
    }
}