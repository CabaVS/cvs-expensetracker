using System.Net;
using CabaVS.ExpenseTracker.Application.Features.Workspaces.Commands;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Presentation.Extensions;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CabaVS.ExpenseTracker.Presentation.Endpoints.Workspaces;

internal sealed class UpdateWorkspaceEndpoint(ISender sender)
    : Endpoint<
        UpdateWorkspaceEndpoint.RequestModel,
        Results<Ok, BadRequest<Error>>>
{
    public override void Configure()
    {
        Put("api/workspaces/{workspaceId:guid}");
        Options(x =>
        {
            x.WithName(nameof(UpdateWorkspaceEndpoint));
            x.WithTags(EndpointTags.Workspaces);
        });
    }
    
    public override async Task<Results<Ok, BadRequest<Error>>> ExecuteAsync(
        RequestModel req,
        CancellationToken ct)
    {
        var command = new UpdateWorkspaceCommand(req.WorkspaceId, req.Name);

        var result = await sender.Send(command, ct);
        
        return result.ToDefaultApiResponse();
    }
    
    internal sealed record RequestModel(Guid WorkspaceId, string Name);
}

internal sealed class UpdateWorkspaceEndpointSummary : Summary<UpdateWorkspaceEndpoint>
{
    public UpdateWorkspaceEndpointSummary()
    {
        Summary = "Update a Workspace.";
        Description = "Updates a new Workspace.";
        
        Params[nameof(UpdateWorkspaceEndpoint.RequestModel.WorkspaceId)] = 
            "Workspace ID to search by (simple GUID).";

        ExampleRequest =
            new UpdateWorkspaceEndpoint.RequestModel(
                Guid.Empty,
                "My Family Budget 2020 UPD");
        
        Response(
            (int)HttpStatusCode.OK,
            "OK response.");
        
        Response(
            (int)HttpStatusCode.BadRequest,
            "Bad Request with Error.",
            example: new Error(
                "Validation.Unspecified",
                "Unspecified validation error occured. Check your input and try again."));
    }
}