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

internal sealed class DeleteBalanceEndpoint(ISender sender)
    : Endpoint<
        DeleteBalanceEndpoint.RequestModel,
        Results<Ok, BadRequest<Error>>>
{
    public override void Configure()
    {
        Delete("api/workspaces/{workspaceId:guid}/balances/{balanceId:guid}");
        Options(x =>
        {
            x.WithName(nameof(DeleteBalanceEndpoint));
            x.WithTags(EndpointTags.Balances);
        });
    }
    
    public override async Task<Results<Ok, BadRequest<Error>>> ExecuteAsync(
        RequestModel req,
        CancellationToken ct)
    {
        var command = new DeleteBalanceCommand(req.WorkspaceId, req.BalanceId);

        Result result = await sender.Send(command, ct);
        
        return result.ToDefaultApiResponse();
    }
    
    internal sealed record RequestModel(Guid WorkspaceId, Guid BalanceId);
}

internal sealed class DeleteBalanceEndpointSummary : Summary<DeleteBalanceEndpoint>
{
    public DeleteBalanceEndpointSummary()
    {
        Summary = "Delete a Balance.";
        Description = "Deletes an existing Balance.";
        
        Params[nameof(GetBalanceByIdEndpoint.GetBalanceByIdEndpointRequest.WorkspaceId)] = 
            "Workspace ID to verify access.";
        Params[nameof(GetBalanceByIdEndpoint.GetBalanceByIdEndpointRequest.BalanceId)] = 
            "Balance ID to verify access.";
        
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
