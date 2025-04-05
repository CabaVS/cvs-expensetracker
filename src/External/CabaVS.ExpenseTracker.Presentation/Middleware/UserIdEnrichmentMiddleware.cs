using CabaVS.ExpenseTracker.Application.Contracts.Presentation;
using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace CabaVS.ExpenseTracker.Presentation.Middleware;

internal sealed class UserIdEnrichmentMiddleware(ICurrentUserAccessor currentUserAccessor) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (currentUserAccessor.UserId is { } userId)
        {
            using (LogContext.PushProperty("UserId", userId))
            {
                await next(context);
            }
        }
        else
        {
            await next(context);
        }
    }
}
