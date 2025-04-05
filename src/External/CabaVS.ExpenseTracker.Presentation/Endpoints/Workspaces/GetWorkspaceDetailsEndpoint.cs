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

internal sealed class GetWorkspaceDetailsEndpoint(ISender sender, ICurrentUserAccessor currentUserAccessor)
    : Endpoint<
        GetWorkspaceDetailsEndpoint.RequestModel,
        Results<Ok<GetWorkspaceDetailsEndpoint.ResponseModel>, BadRequest<Error>>>
{
    public override void Configure()
    {
        Get("/api/workspaces/{workspaceId:guid}");
        Options(x =>
        {
            x.WithName(nameof(GetWorkspaceDetailsEndpoint));
            x.WithTags(EndpointTags.Workspaces);
        });
    }

    public override async Task<Results<Ok<ResponseModel>, BadRequest<Error>>> ExecuteAsync(
        RequestModel req, CancellationToken ct)
    {
        var query = new GetWorkspaceDetailsByUserQuery(currentUserAccessor.UserId!.Value, req.WorkspaceId);
        
        Result<WorkspaceDetailsModel> result = await sender.Send(query, ct);

        return result.ToDefaultApiResponse(workspace => new ResponseModel(workspace));
    }

    internal sealed record RequestModel(Guid WorkspaceId);
    internal sealed record ResponseModel(WorkspaceDetailsModel Workspace);
    
    internal sealed class EndpointSummary : Summary<GetWorkspaceDetailsEndpoint>
    {
        public EndpointSummary()
        {
            Summary = "Get Workspace details";
            Description = "Gets Workspace details if user has access to it.";
        
            Response(
                (int)HttpStatusCode.OK,
                "OK response with body.",
                example: new ResponseModel(
                    new WorkspaceDetailsModel(
                        Guid.NewGuid(),
                        "My workspace 2023",
                        [
                            new WorkspaceMemberModel(
                                Guid.NewGuid(),
                                true,
                                Guid.NewGuid(),
                                "Admin of Workspace"),
                            new WorkspaceMemberModel(
                                Guid.NewGuid(),
                                false,
                                Guid.NewGuid(),
                                "Member of Workspace")
                        ])));
        
            Response(
                (int)HttpStatusCode.BadRequest,
                "Bad Request with Error.",
                example: new Error(
                    "Validation.Unspecified",
                    "Unspecified validation error occured. Check your input and try again."));
        }
    }
}
