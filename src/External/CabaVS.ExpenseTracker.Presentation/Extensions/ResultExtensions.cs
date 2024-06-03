using CabaVS.ExpenseTracker.Domain.Shared;
using Microsoft.AspNetCore.Http;

namespace CabaVS.ExpenseTracker.Presentation.Extensions;

internal static class ResultExtensions
{
    public static IResult ToDefaultApiResponse(this Result result) =>
        result.IsSuccess
            ? Results.Ok()
            : Results.BadRequest(result.Error);
    
    public static IResult ToDefaultApiResponse<T>(this Result<T> result) =>
        result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.BadRequest(result.Error);
    
    public static IResult ToDefaultApiResponse(this Result<Guid> result, string routeName) =>
        result.IsSuccess
            ? Results.CreatedAtRoute(routeName, new { Id = result.Value })
            : Results.BadRequest(result.Error);
}