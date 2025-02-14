using System.Net;
using CabaVS.ExpenseTracker.Application.Features.Balances.Commands;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Presentation.Extensions;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CabaVS.ExpenseTracker.Presentation.Endpoints.Balances;

internal sealed class UpdateBalanceEndpoint(ISender sender)
    : Endpoint<
        UpdateBalanceEndpoint.RequestModel,
        Results<Ok, BadRequest<Error>>>
{
    public override void Configure()
    {
        Put("api/workspaces/{workspaceId:guid}/balances/{balanceId:guid}");
        Options(x =>
        {
            x.WithName(nameof(UpdateBalanceEndpoint));
            x.WithTags(EndpointTags.Balances);
        });
    }
    
    public override async Task<Results<Ok, BadRequest<Error>>> ExecuteAsync(
        RequestModel req,
        CancellationToken ct)
    {
        var command = new UpdateBalanceCommand(req.WorkspaceId, req.BalanceId, req.Name, req.Amount);

        Result result = await sender.Send(command, ct);
        
        return result.ToDefaultApiResponse();
    }
    
    internal sealed record RequestModel(Guid WorkspaceId, Guid BalanceId, string Name, decimal Amount);
}

internal sealed class UpdateBalanceEndpointSummary : Summary<UpdateBalanceEndpoint>
{
    public UpdateBalanceEndpointSummary()
    {
        Summary = "Update a Balance.";
        Description = "Updates an existing Balance.";
        
        Params[nameof(GetBalanceByIdEndpoint.GetBalanceByIdEndpointRequest.WorkspaceId)] = 
            "Workspace ID to verify access.";
        Params[nameof(GetBalanceByIdEndpoint.GetBalanceByIdEndpointRequest.BalanceId)] = 
            "Balance ID to verify access.";

        ExampleRequest =
            new UpdateBalanceEndpoint.RequestModel(
                Guid.Empty,
                Guid.Empty,
                "My USD card UPDATED",
                9999.99m);
        
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
