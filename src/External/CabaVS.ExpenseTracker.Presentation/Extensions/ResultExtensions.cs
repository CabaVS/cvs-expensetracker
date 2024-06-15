using CabaVS.ExpenseTracker.Domain.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CabaVS.ExpenseTracker.Presentation.Extensions;

internal static class ResultExtensions
{
    public static Results<Ok, BadRequest<Error>> ToDefaultApiResponse(this Result result) =>
        result.IsSuccess
            ? TypedResults.Ok()
            : TypedResults.BadRequest(result.Error);
    
    public static Results<Ok<T>, BadRequest<Error>> ToDefaultApiResponse<T>(this Result<T> result) =>
        result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : TypedResults.BadRequest(result.Error);
    
    public static Results<CreatedAtRoute, BadRequest<Error>> ToDefaultApiResponse(
        this Result<Guid> result,
        string routeName,
        Func<Guid, object>? routeValuesFactory = null)
    {
        routeValuesFactory ??= id => new { id }; 
        
        return result.IsSuccess
            ? TypedResults.CreatedAtRoute(routeName, routeValuesFactory(result.Value))
            : TypedResults.BadRequest(result.Error);
    }
}