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
        Post("api/workspaces");
        Options(x =>
        {
            x.WithName(nameof(CreateWorkspaceEndpoint));
            x.WithTags(EndpointTags.Workspaces);
        });
    }
    
    public override async Task<Results<CreatedAtRoute, BadRequest<Error>>> ExecuteAsync(
        RequestModel req,
        CancellationToken ct)
    {
        var command = new CreateWorkspaceCommand(req.Name);

        Result<Guid> result = await sender.Send(command, ct);
        
        return result.ToDefaultApiResponse(
            nameof(GetWorkspaceByIdEndpoint),
            id => new { WorkspaceId = id });
    }
    
    internal sealed record RequestModel(string Name);
}

internal sealed class CreateWorkspaceEndpointSummary : Summary<CreateWorkspaceEndpoint>
{
    public CreateWorkspaceEndpointSummary()
    {
        Summary = "Create a Workspace.";
        Description = "Creates a new Workspace.";

        ExampleRequest =
            new CreateWorkspaceEndpoint.RequestModel(
                "My Family Budget 2020");
        
        Response(
            (int)HttpStatusCode.Created,
            "Created At response with Location header filled.");
        
        Response(
            (int)HttpStatusCode.BadRequest,
            "Bad Request with Error.",
            example: new Error(
                "Validation.Unspecified",
                "Unspecified validation error occured. Check your input and try again."));
    }
}
