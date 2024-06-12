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

internal sealed class GetWorkspaceByIdEndpoint(ISender sender) 
    : Endpoint<
        GetWorkspaceByIdEndpoint.GetWorkspaceByIdEndpointRequest,
        Results<Ok<WorkspaceModel>, BadRequest<Error>>>
{
    public override void Configure()
    {
        AllowAnonymous();
        Get("api/workspaces/{id:guid}");
        Options(x =>
        {
            x.WithName(nameof(GetWorkspaceByIdEndpoint));
            x.WithTags(EndpointTags.Workspaces);
        });
    }

    public override async Task<Results<Ok<WorkspaceModel>, BadRequest<Error>>> ExecuteAsync(
        GetWorkspaceByIdEndpointRequest req,
        CancellationToken ct)
    {
        var query = new GetWorkspaceByIdQuery(req.Id);

        var result = await sender.Send(query, ct);

        return result.ToDefaultApiResponse();
    }

    internal sealed record GetWorkspaceByIdEndpointRequest(Guid Id);
}

internal sealed class GetWorkspaceByIdEndpointSummary : Summary<GetWorkspaceByIdEndpoint>
{
    public GetWorkspaceByIdEndpointSummary()
    {
        Summary = "Get Workspace by ID";
        Description = "Gets a Workspace by provided ID.";

        Params[nameof(GetWorkspaceByIdEndpoint.GetWorkspaceByIdEndpointRequest.Id)] = 
            "Workspace ID to search by (simple GUID).";
        
        Response(
            (int)HttpStatusCode.OK,
            "OK response with body.",
            example: new WorkspaceModel(
                new Guid("DCF2732A-E43A-4CB1-8718-EA1AD3543BA1"),
                "My Family Budget 2020",
                true));
        
        Response(
            (int)HttpStatusCode.BadRequest,
            "Bad Request with Error.",
            example: new Error(
                "Validation.Unspecified",
                "Unspecified validation error occured. Check your input and try again."));
    }
}