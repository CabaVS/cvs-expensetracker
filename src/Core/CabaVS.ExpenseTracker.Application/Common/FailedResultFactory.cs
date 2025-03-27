using CabaVS.ExpenseTracker.Domain.Shared;

namespace CabaVS.ExpenseTracker.Application.Common;

internal static class FailedResultFactory
{
    public static TResponse Create<TResponse>(Error error) where TResponse : Result
    {
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
