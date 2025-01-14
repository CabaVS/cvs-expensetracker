using System.Net;
using CabaVS.ExpenseTracker.Application.Features.Workspaces.Models;
using CabaVS.ExpenseTracker.Application.Features.Workspaces.Queries;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Presentation.Extensions;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CabaVS.ExpenseTracker.Presentation.Endpoints.Workspaces;

internal sealed class GetWorkspacesEndpoint(ISender sender) 
    : EndpointWithoutRequest<
        Results<Ok<WorkspaceModel[]>, BadRequest<Error>>>
{
    public override void Configure()
    {
        Get("api/workspaces");
        Options(x =>
        {
            x.WithName(nameof(GetWorkspacesEndpoint));
            x.WithTags(EndpointTags.Workspaces);
        });
    }

    public override async Task<Results<Ok<WorkspaceModel[]>, BadRequest<Error>>> ExecuteAsync(
        CancellationToken ct)
    {
        var query = new GetWorkspacesQuery();

        Result<WorkspaceModel[]> result = await sender.Send(query, ct);

        return result.ToDefaultApiResponse();
    }
}

internal sealed class GetWorkspacesEndpointSummary : Summary<GetWorkspacesEndpoint>
{
    public GetWorkspacesEndpointSummary()
    {
        Summary = "Get Workspaces";
        Description = "Gets Workspaces. Ensures that User has access over these Workspaces.";
        
        Response(
            (int)HttpStatusCode.OK,
            "OK response with body.",
            example: new WorkspaceModel[]
            {
                new(
                    new Guid("DCF2732A-E43A-4CB1-8718-EA1AD3543BA1"),
                    "My Family Budget 2020",
                    true),
                new(
                    new Guid("03B7A4E8-B0BC-4F73-B68C-E31EE6956D62"),
                    "My Family Budget 2021",
                    false)
            });
        
        Response(
            (int)HttpStatusCode.BadRequest,
            "Bad Request with Error.",
            example: new Error(
                "Validation.Unspecified",
                "Unspecified validation error occured. Check your input and try again."));
    }
}
