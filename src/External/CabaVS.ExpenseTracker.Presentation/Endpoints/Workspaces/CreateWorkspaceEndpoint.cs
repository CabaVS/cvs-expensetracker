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

internal sealed class CreateWorkspaceEndpoint(ISender sender)
    : Endpoint<
        CreateWorkspaceEndpoint.RequestModel,
        Results<CreatedAtRoute, BadRequest<Error>>>
{
    public override void Configure()
    {
        Post("/api/workspaces");
        Options(x =>
        {
            x.WithName(nameof(CreateWorkspaceEndpoint));
            x.WithTags(EndpointTags.Workspaces);
        });
    }

    public override async Task<Results<CreatedAtRoute, BadRequest<Error>>> ExecuteAsync(
        RequestModel req, CancellationToken ct)
    {
        var command = new CreateWorkspaceCommand(req.WorkspaceName);
        
        Result<Guid> result = await sender.Send(command, ct);

        return result.ToDefaultApiResponse(nameof(GetWorkspaceDetailsEndpoint), id => new { WorkspaceId = id });
    }

    internal sealed record RequestModel(string WorkspaceName);
    
    internal sealed class EndpointSummary : Summary<CreateWorkspaceEndpoint>
    {
        public EndpointSummary()
        {
            Summary = "Create a Workspace";
            Description = "Creates a Workspace.";

            ExampleRequest =
                new RequestModel(
                    "My Workspace");
        
            Response(
                (int)HttpStatusCode.Created,
                "Created At Route response without a body. Location header is filled with Id of created entity.");
        
            Response(
                (int)HttpStatusCode.BadRequest,
                "Bad Request with Error.",
                example: new Error(
                    "Validation.Unspecified",
                    "Unspecified validation error occured. Check your input and try again."));
        }
    }
}
