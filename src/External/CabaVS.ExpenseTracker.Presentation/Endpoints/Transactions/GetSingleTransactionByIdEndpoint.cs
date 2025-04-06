using System.Net;
using CabaVS.ExpenseTracker.Application.Features.Transactions.Models;
using CabaVS.ExpenseTracker.Application.Features.Transactions.Queries;
using CabaVS.ExpenseTracker.Domain.Enums;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Presentation.Extensions;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CabaVS.ExpenseTracker.Presentation.Endpoints.Transactions;

internal sealed class GetSingleTransactionByIdEndpoint(ISender sender)
    : Endpoint<
        GetSingleTransactionByIdEndpoint.RequestModel,
        Results<Ok<GetSingleTransactionByIdEndpoint.ResponseModel>, BadRequest<Error>>>
{
    public override void Configure()
    {
        Get("/api/workspaces/{workspaceId:guid}/transactions/{transactionId:guid}");
        Options(x =>
        {
            x.WithName(nameof(GetSingleTransactionByIdEndpoint));
            x.WithTags(EndpointTags.Transactions);
        });
    }

    public override async Task<Results<Ok<ResponseModel>, BadRequest<Error>>> ExecuteAsync(
        RequestModel req, CancellationToken ct)
    {
        var query = new GetSingleTransactionByIdQuery(req.WorkspaceId, req.TransactionId);
        
        Result<TransactionModel> result = await sender.Send(query, ct);

        return result.ToDefaultApiResponse(transactionModel => new ResponseModel(transactionModel));
    }

    internal sealed record RequestModel(Guid WorkspaceId, Guid TransactionId);
    internal sealed record ResponseModel(TransactionModel Transaction);
    
    internal sealed class EndpointSummary : Summary<GetSingleTransactionByIdEndpoint>
    {
        public EndpointSummary()
        {
            Summary = "Get a Transaction by ID";
            Description = "Get a Transaction by ID within specified workspace.";
        
            Response(
                (int)HttpStatusCode.OK,
                "OK response with body.",
                example: new ResponseModel(
                    new TransactionModel(
                        Guid.NewGuid(),
                        DateOnly.FromDateTime(DateTime.UtcNow),
                        TransactionType.Expense,
                        ["expense","household"],
                        1234.56m,
                        1234.56m,
                        new TransactionSideModel(
                            Guid.NewGuid(),
                            "My Card"),
                        new TransactionSideModel(
                            Guid.NewGuid(),
                            "Household category"))));
        
            Response(
                (int)HttpStatusCode.BadRequest,
                "Bad Request with Error.",
                example: new Error(
                    "Validation.Unspecified",
                    "Unspecified validation error occured. Check your input and try again."));
        }
    }
}
