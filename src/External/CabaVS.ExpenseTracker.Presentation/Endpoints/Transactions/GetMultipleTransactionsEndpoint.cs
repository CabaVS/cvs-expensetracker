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

internal sealed class GetMultipleTransactionsEndpoint(ISender sender)
    : Endpoint<
        GetMultipleTransactionsEndpoint.RequestModel,
        Results<Ok<GetMultipleTransactionsEndpoint.ResponseModel>, BadRequest<Error>>>
{
    public override void Configure()
    {
        Get("/api/workspaces/{workspaceId:guid}/transactions");
        Options(x =>
        {
            x.WithName(nameof(GetMultipleTransactionsEndpoint));
            x.WithTags(EndpointTags.Transactions);
        });
    }

    public override async Task<Results<Ok<ResponseModel>, BadRequest<Error>>> ExecuteAsync(
        RequestModel req, CancellationToken ct)
    {
        var query = new GetMultipleTransactionsQuery(req.WorkspaceId, req.From, req.To);
        
        Result<TransactionModel[]> result = await sender.Send(query, ct);

        return result.ToDefaultApiResponse(transactionModels => new ResponseModel(transactionModels));
    }

    internal sealed record RequestModel(Guid WorkspaceId, DateOnly From, DateOnly To);
    internal sealed record ResponseModel(TransactionModel[] Transactions);
    
    internal sealed class EndpointSummary : Summary<GetMultipleTransactionsEndpoint>
    {
        public EndpointSummary()
        {
            Summary = "Get multiple Transactions";
            Description = "Gets multiple Transactions within a specified workspace for a specified time period.";
        
            Response(
                (int)HttpStatusCode.OK,
                "OK response with body.",
                example: new ResponseModel(
                [
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
                            "Household category")),
                    new TransactionModel(
                        Guid.NewGuid(),
                        DateOnly.FromDateTime(DateTime.UtcNow),
                        TransactionType.Income,
                        ["income","paycheck"],
                        1234.56m,
                        1234.56m,
                        new TransactionSideModel(
                            Guid.NewGuid(),
                            "My Company"),
                        new TransactionSideModel(
                            Guid.NewGuid(),
                            "My Card"))
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
