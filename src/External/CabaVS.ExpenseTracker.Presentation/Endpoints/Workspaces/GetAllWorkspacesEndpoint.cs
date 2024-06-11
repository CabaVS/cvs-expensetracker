using System.Diagnostics;
using System.Net;
using CabaVS.ExpenseTracker.Application.Features.Workspaces.Models;
using CabaVS.ExpenseTracker.Application.Features.Workspaces.Queries;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CabaVS.ExpenseTracker.Presentation.Endpoints.Workspaces;

internal sealed class GetAllWorkspacesEndpoint(ISender sender) : EndpointWithoutRequest<Ok<WorkspaceModel[]>>
{
    public override void Configure()
    {
        AllowAnonymous();
        Get("api/workspaces");
        Options(x => x.WithName(nameof(GetAllWorkspacesEndpoint)));
    }

    public override async Task<Ok<WorkspaceModel[]>> ExecuteAsync(CancellationToken ct)
    {
        var query = new GetAllWorkspacesQuery();

        var result = await sender.Send(query, ct);
        if (result.IsFailure)
        {
            throw new UnreachableException();
        }

        return TypedResults.Ok(result.Value);
    }
}

internal sealed class GetAllWorkspacesEndpointSummary : Summary<GetAllWorkspacesEndpoint>
{
    public GetAllWorkspacesEndpointSummary()
    {
        Summary = "Get all Workspaces";
        Description = "Gets all Workspaces.";
        
        Response(
            (int)HttpStatusCode.OK,
            "OK response with body.",
            example: new[]
            {
                new WorkspaceModel(
                    new Guid("AE6320FD-AE15-44AE-B65D-18C6541042DD"),
                    "My Family Budget 2020",
                    true),
                new WorkspaceModel(
                    new Guid("8DC3B3C7-E1C6-4192-87A6-4A67192ADB94"),
                    "Company 2022",
                    false)
            });
    }
}