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
        AllowAnonymous();
        Put("api/workspaces/{workspaceId:guid}/balances/{balanceId}");
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
        var command = new UpdateBalanceCommand(
            req.WorkspaceId,
            req.BalanceId,
            req.Name,
            req.Amount);

        var result = await sender.Send(command, ct);
        
        return result.ToDefaultApiResponse();
    }
    
    internal sealed record RequestModel(Guid WorkspaceId, Guid BalanceId, string Name, decimal Amount);
}

internal sealed class UpdateBalanceEndpointSummary : Summary<UpdateBalanceEndpoint>
{
    public UpdateBalanceEndpointSummary()
    {
        Summary = "Update a Balance.";
        Description = "Updates a new Balance.";
        
        Params[nameof(UpdateBalanceEndpoint.RequestModel.WorkspaceId)] = 
            "Workspace ID (simple GUID, user should have access to it).";
        
        Params[nameof(UpdateBalanceEndpoint.RequestModel.BalanceId)] = 
            "Balance ID to search by (simple GUID).";

        ExampleRequest =
            new UpdateBalanceEndpoint.RequestModel(
                Guid.Empty,
                Guid.Empty,
                "Visa EU",
                1000);
        
        Response(
            (int)HttpStatusCode.OK,
            "OK response without body.");
        
        Response(
            (int)HttpStatusCode.BadRequest,
            "Bad Request with Error.",
            example: new Error(
                "Validation.Unspecified",
                "Unspecified validation error occured. Check your input and try again."));
    }
}