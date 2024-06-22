using System.Net;
using CabaVS.ExpenseTracker.Application.Features.Transactions.Models;
using CabaVS.ExpenseTracker.Application.Features.Transactions.Queries;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Presentation.Extensions;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CabaVS.ExpenseTracker.Presentation.Endpoints.IncomeTransactions;

internal sealed class GetIncomeTransactionByIdEndpoint(ISender sender) 
    : Endpoint<
        GetIncomeTransactionByIdEndpoint.RequestModel,
        Results<Ok<IncomeTransactionModel>, BadRequest<Error>>>
{
    public override void Configure()
    {
        AllowAnonymous();
        Get("api/workspaces/{workspaceId:guid}/income-transactions/{incomeTransactionId:guid}");
        Options(x =>
        {
            x.WithName(nameof(GetIncomeTransactionByIdEndpoint));
            x.WithTags(EndpointTags.IncomeTransactions);
        });
    }

    public override async Task<Results<Ok<IncomeTransactionModel>, BadRequest<Error>>> ExecuteAsync(
        RequestModel req,
        CancellationToken ct)
    {
        var query = new GetIncomeTransactionByIdQuery(req.IncomeTransactionId, req.WorkspaceId);

        var result = await sender.Send(query, ct);

        return result.ToDefaultApiResponse();
    }

    internal sealed record RequestModel(Guid WorkspaceId, Guid IncomeTransactionId);
}

internal sealed class GetIncomeTransactionByIdEndpointSummary : Summary<GetIncomeTransactionByIdEndpoint>
{
    public GetIncomeTransactionByIdEndpointSummary()
    {
        Summary = "Get Income Transaction by ID";
        Description = "Gets an Income Transaction by provided ID.";

        Params[nameof(GetIncomeTransactionByIdEndpoint.RequestModel.WorkspaceId)] = 
            "Workspace ID (simple GUID, user should have access to it).";
        Params[nameof(GetIncomeTransactionByIdEndpoint.RequestModel.IncomeTransactionId)] = 
            "Income Transaction ID to search by (simple GUID).";
        
        Response(
            (int)HttpStatusCode.OK,
            "OK response with body.",
            example: new IncomeTransactionModel(
                new Guid("0D64417A-AC39-44C5-857A-01BECE08700D"),
                new DateOnly(2020, 10, 20),
                new CategoryUnderTransactionModel(
                    new Guid("0D64417A-AC39-44C5-857A-01BECE08700D"),
                    "My Company"),
                1234.56m,
                new BalanceUnderTransactionModel(
                    new Guid("E650E18C-0069-45B3-B56A-AB8C3BA8D8AA"),
                    "Card USD"),
                1234.56m,
                ["salary"]));
        
        Response(
            (int)HttpStatusCode.BadRequest,
            "Bad Request with Error.",
            example: new Error(
                "Validation.Unspecified",
                "Unspecified validation error occured. Check your input and try again."));
    }
}