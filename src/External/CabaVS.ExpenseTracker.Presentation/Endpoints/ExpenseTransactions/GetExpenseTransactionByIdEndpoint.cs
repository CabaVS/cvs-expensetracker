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

namespace CabaVS.ExpenseTracker.Presentation.Endpoints.ExpenseTransactions;

internal sealed class GetExpenseTransactionByIdEndpoint(ISender sender) 
    : Endpoint<
        GetExpenseTransactionByIdEndpoint.RequestModel,
        Results<Ok<ExpenseTransactionModel>, BadRequest<Error>>>
{
    public override void Configure()
    {
        AllowAnonymous();
        Get("api/workspaces/{workspaceId:guid}/expense-transactions/{expenseTransactionId:guid}");
        Options(x =>
        {
            x.WithName(nameof(GetExpenseTransactionByIdEndpoint));
            x.WithTags(EndpointTags.ExpenseTransactions);
        });
    }

    public override async Task<Results<Ok<ExpenseTransactionModel>, BadRequest<Error>>> ExecuteAsync(
        RequestModel req,
        CancellationToken ct)
    {
        var query = new GetExpenseTransactionByIdQuery(req.ExpenseTransactionId, req.WorkspaceId);

        var result = await sender.Send(query, ct);

        return result.ToDefaultApiResponse();
    }

    internal sealed record RequestModel(Guid WorkspaceId, Guid ExpenseTransactionId);
}

internal sealed class GetExpenseTransactionByIdEndpointSummary : Summary<GetExpenseTransactionByIdEndpoint>
{
    public GetExpenseTransactionByIdEndpointSummary()
    {
        Summary = "Get Expense Transaction by ID";
        Description = "Gets an Expense Transaction by provided ID.";

        Params[nameof(GetExpenseTransactionByIdEndpoint.RequestModel.WorkspaceId)] = 
            "Workspace ID (simple GUID, user should have access to it).";
        Params[nameof(GetExpenseTransactionByIdEndpoint.RequestModel.ExpenseTransactionId)] = 
            "Expense Transaction ID to search by (simple GUID).";
        
        Response(
            (int)HttpStatusCode.OK,
            "OK response with body.",
            example: new ExpenseTransactionModel(
                new Guid("0D64417A-AC39-44C5-857A-01BECE08700D"),
                new DateOnly(2020, 10, 20),
                new BalanceUnderTransactionModel(
                    new Guid("E650E18C-0069-45B3-B56A-AB8C3BA8D8AA"),
                    "Card USD"),
                1234.56m,
                new CategoryUnderTransactionModel(
                    new Guid("0D64417A-AC39-44C5-857A-01BECE08700D"),
                    "Transportation"),
                1234.56m,
                ["uber", "taxi"]));
        
        Response(
            (int)HttpStatusCode.BadRequest,
            "Bad Request with Error.",
            example: new Error(
                "Validation.Unspecified",
                "Unspecified validation error occured. Check your input and try again."));
    }
}