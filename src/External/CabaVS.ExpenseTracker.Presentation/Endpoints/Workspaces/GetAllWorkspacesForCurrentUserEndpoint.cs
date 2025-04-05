using System.Net;
using CabaVS.ExpenseTracker.Application.Contracts.Presentation;
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

internal sealed class GetAllWorkspacesForCurrentUserEndpoint(ISender sender, ICurrentUserAccessor currentUserAccessor)
    : EndpointWithoutRequest<
        Results<Ok<GetAllWorkspacesForCurrentUserEndpoint.ResponseModel>, BadRequest<Error>>>
{
    public override void Configure()
    {
        Get("/api/workspaces");
        Options(x =>
        {
            x.WithName(nameof(GetAllWorkspacesForCurrentUserEndpoint));
            x.WithTags(EndpointTags.Workspaces);
        });
    }

    public override async Task<Results<Ok<ResponseModel>, BadRequest<Error>>> ExecuteAsync(CancellationToken ct)
    {
        var query = new GetAllWorkspacesByUserQuery(currentUserAccessor.UserId!.Value);
        
        Result<WorkspaceModel[]> result = await sender.Send(query, ct);

        return result.ToDefaultApiResponse(workspaces => new ResponseModel(workspaces));
    }
    
    internal sealed record ResponseModel(WorkspaceModel[] Workspaces);
    
    internal sealed class EndpointSummary : Summary<GetAllWorkspacesForCurrentUserEndpoint>
    {
        public EndpointSummary()
        {
            Summary = "Get all Workspaces";
            Description = "Get all Workspaces for a current user.";
        
            Response(
                (int)HttpStatusCode.OK,
                "OK response with body.",
                example: new ResponseModel(
                    [
                        new WorkspaceModel(
                            Guid.NewGuid(),
                            "My workspace 2023"),
                        new WorkspaceModel(
                            Guid.NewGuid(),
                            "My workspace 2024")
                    ]));
        
            Response(
                (int)HttpStatusCode.BadRequest,
                "Bad Request with Error.",
                example: new Error(
                    "Validation.Unspecified",
                    "Unspecified validation error occured. Check your input and try again."));
        }
    }
}
