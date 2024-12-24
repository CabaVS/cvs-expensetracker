using System.Net;
using CabaVS.ExpenseTracker.Application.Features.TransferTransactions.Models;
using CabaVS.ExpenseTracker.Application.Features.TransferTransactions.Queries;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Presentation.Extensions;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CabaVS.ExpenseTracker.Presentation.Endpoints.TransferTransactions;

internal sealed class GetTransferTransactionByIdEndpoint(ISender sender)
    : Endpoint<
        GetTransferTransactionByIdEndpoint.GetTransferTransactionByIdEndpointRequest,
        Results<Ok<TransferTransactionModel>, BadRequest<Error>>>
{
    public override void Configure()
    {
        Get("api/workspaces/{workspaceId:guid}/transfer-transactions/{transferTransactionId:guid}");
        Options(x =>
        {
            x.WithName(nameof(GetTransferTransactionByIdEndpoint));
            x.WithTags(EndpointTags.TransferTransactions);
        });
    }

    public override async Task<Results<Ok<TransferTransactionModel>, BadRequest<Error>>> ExecuteAsync(
        GetTransferTransactionByIdEndpointRequest req,
        CancellationToken ct)
    {
        var query = new GetByIdTransferTransactionQuery(req.WorkspaceId, req.TransferTransactionId);

        var result = await sender.Send(query, ct);

        return result.ToDefaultApiResponse();
    }

    internal sealed record GetTransferTransactionByIdEndpointRequest(Guid WorkspaceId, Guid TransferTransactionId);
}

internal sealed class GetTransferTransactionByIdEndpointSummary : Summary<GetTransferTransactionByIdEndpoint>
{
    public GetTransferTransactionByIdEndpointSummary()
    {
        Summary = "Get Transfer Transaction by ID";
        Description = "Gets a Transfer Transaction by provided ID. Ensures that User is a member of the parent Workspace.";

        Params[nameof(GetTransferTransactionByIdEndpoint.GetTransferTransactionByIdEndpointRequest.WorkspaceId)] = 
            "Workspace ID to verify access.";
        Params[nameof(GetTransferTransactionByIdEndpoint.GetTransferTransactionByIdEndpointRequest.TransferTransactionId)] = 
            "Transfer Transaction ID to search by (simple GUID).";
        
        Response(
            (int)HttpStatusCode.OK,
            "OK response with body.",
            example: new TransferTransactionModel(
                new Guid("DCF2732A-E43A-4CB1-8718-EA1AD3543BA1"),
                new DateOnly(2020, 10, 20),
                ["taxi", "uber"],
                123.45m,
                new TransferTransactionModel.CurrencyModel(
                    new Guid("4BEECA0B-4AFD-4D40-8D15-BFB8C4D3360F"),
                    "USD"),
                123.45m,
                new TransferTransactionModel.BalanceModel(
                    new Guid("7BB89C39-9799-4E39-A3AC-067C5BF34DEA"),
                    new TransferTransactionModel.CurrencyModel(
                        new Guid("4BEECA0B-4AFD-4D40-8D15-BFB8C4D3360F"),
                        "USD")),
                678.90m,
                new TransferTransactionModel.BalanceModel(
                    new Guid("DB251B4C-B00C-473F-B75F-433AA6DD2977"),
                    new TransferTransactionModel.CurrencyModel(
                        new Guid("0D3F3CCE-C8A3-42F6-B51D-ADCB48AA36E0"),
                        "PLN"))));
        
        Response(
            (int)HttpStatusCode.BadRequest,
            "Bad Request with Error.",
            example: new Error(
                "Validation.Unspecified",
                "Unspecified validation error occured. Check your input and try again."));
    }
}