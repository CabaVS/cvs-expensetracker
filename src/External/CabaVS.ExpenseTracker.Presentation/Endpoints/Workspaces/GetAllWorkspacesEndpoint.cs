using CabaVS.ExpenseTracker.Presentation.Auth;
using FastEndpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CabaVS.ExpenseTracker.Presentation.Endpoints.Workspaces;

internal sealed class GetAllWorkspacesEndpoint : EndpointWithoutRequest<Ok>
{
    public override void Configure()
    {
        Get("api/workspaces");
        Policies(PolicyNames.User);
        
        Options(builder =>
        {
            builder.WithName(nameof(GetAllWorkspacesEndpoint));
            builder.WithTags(EndpointTags.Workspaces);
        });
    }

    public override async Task<Ok> ExecuteAsync(CancellationToken ct)
    {
        return TypedResults.Ok();
    }
}

internal sealed class GetAllWorkspacesEndpointSummary : Summary<GetAllWorkspacesEndpoint>
{
    public GetAllWorkspacesEndpointSummary()
    {
        Summary = "Get all Workspaces";
        Description = "Gets all Workspaces.";
    }   
}