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

internal sealed class DeleteWorkspaceEndpoint(ISender sender)
    : Endpoint<
        DeleteWorkspaceEndpoint.RequestModel,
        Results<Ok, BadRequest<Error>>>
{
    public override void Configure()
    {
        Delete("api/workspaces/{workspaceId:guid}");
        Options(x =>
        {
            x.WithName(nameof(DeleteWorkspaceEndpoint));
            x.WithTags(EndpointTags.Workspaces);
        });
    }
    
    public override async Task<Results<Ok, BadRequest<Error>>> ExecuteAsync(
        RequestModel req,
        CancellationToken ct)
    {
        var command = new DeleteWorkspaceCommand(req.WorkspaceId);

        var result = await sender.Send(command, ct);
        
        return result.ToDefaultApiResponse();
    }
    
    internal sealed record RequestModel(Guid WorkspaceId);
}

internal sealed class DeleteWorkspaceEndpointSummary : Summary<DeleteWorkspaceEndpoint>
{
    public DeleteWorkspaceEndpointSummary()
    {
        Summary = "Delete a Workspace.";
        Description = "Deletes a new Workspace.";
        
        Params[nameof(DeleteWorkspaceEndpoint.RequestModel.WorkspaceId)] = 
            "Workspace ID to search by (simple GUID).";
        
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