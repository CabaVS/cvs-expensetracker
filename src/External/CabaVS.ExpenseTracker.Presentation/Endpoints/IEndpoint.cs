using Microsoft.AspNetCore.Routing;

namespace CabaVS.ExpenseTracker.Presentation.Endpoints;

internal interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}