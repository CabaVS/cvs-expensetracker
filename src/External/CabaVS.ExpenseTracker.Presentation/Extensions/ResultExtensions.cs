using CabaVS.ExpenseTracker.Domain.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CabaVS.ExpenseTracker.Presentation.Extensions;

internal static class ResultExtensions
{
    internal static Results<Ok<TResponse>, BadRequest<Error>> ToDefaultApiResponse<TResult, TResponse>(
        this Result<TResult> result,
        Func<TResult, TResponse> mappingFunc) =>
        result.IsSuccess
            ? TypedResults.Ok(mappingFunc(result.Value))
            : TypedResults.BadRequest(result.Error);
}
