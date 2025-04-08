using System.Net;
using CabaVS.ExpenseTracker.Application.Features.Transactions.Commands;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Presentation.Extensions;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CabaVS.ExpenseTracker.Presentation.Endpoints.Transactions;

internal sealed class DeleteTransactionEndpoint(ISender sender)
    : Endpoint<
        DeleteTransactionEndpoint.RequestModel,
        Results<Ok, BadRequest<Error>>>
{
    public override void Configure()
    {
        Delete("/api/workspaces/{workspaceId}/transactions/{transactionId}");
        Options(x =>
        {
            x.WithName(nameof(DeleteTransactionEndpoint));
            x.WithTags(EndpointTags.Transactions);
        });
    }

    public override async Task<Results<Ok, BadRequest<Error>>> ExecuteAsync(
        RequestModel req, CancellationToken ct)
    {
        var command = new DeleteTransactionCommand(req.WorkspaceId, req.TransactionId);
        
        Result result = await sender.Send(command, ct);

        return result.ToDefaultApiResponse();
    }

    internal sealed record RequestModel(Guid WorkspaceId, Guid TransactionId);
    
    internal sealed class EndpointSummary : Summary<DeleteTransactionEndpoint>
    {
        public EndpointSummary()
        {
            Summary = "Create a Transaction";
            Description = "Creates a Transaction within specified workspace.";
            
            Params[nameof(RequestModel.WorkspaceId)] = "Workspace ID to verify access.";
            Params[nameof(RequestModel.TransactionId)] = "Transaction ID to delete.";
        
            Response(
                (int)HttpStatusCode.OK,
                "OK response without a body.");
        
            Response(
                (int)HttpStatusCode.BadRequest,
                "Bad Request with Error.",
                example: new Error(
                    "Validation.Unspecified",
                    "Unspecified validation error occured. Check your input and try again."));
        }
    }
}
