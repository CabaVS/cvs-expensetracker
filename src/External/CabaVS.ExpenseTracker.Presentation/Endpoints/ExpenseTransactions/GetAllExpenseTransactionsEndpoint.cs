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

internal sealed class GetAllExpenseTransactionsEndpoint(ISender sender) 
    : Endpoint<
        GetAllExpenseTransactionsEndpoint.RequestModel,
        Results<Ok<ExpenseTransactionModel[]>, BadRequest<Error>>>
{
    public override void Configure()
    {
        Get("api/workspaces/{workspaceId:guid}/expense-transactions");
        Options(x =>
        {
            x.WithName(nameof(GetAllExpenseTransactionsEndpoint));
            x.WithTags(EndpointTags.ExpenseTransactions);
        });
    }

    public override async Task<Results<Ok<ExpenseTransactionModel[]>, BadRequest<Error>>> ExecuteAsync(
        RequestModel req,
        CancellationToken ct)
    {
        var query = new GetAllExpenseTransactionsQuery(req.WorkspaceId);

        var result = await sender.Send(query, ct);

        return result.ToDefaultApiResponse();
    }

    internal sealed record RequestModel(Guid WorkspaceId);
}

internal sealed class GetAllExpenseTransactionsEndpointSummary : Summary<GetAllExpenseTransactionsEndpoint>
{
    public GetAllExpenseTransactionsEndpointSummary()
    {
        Summary = "Get all Expense Transactions";
        Description = "Gets all Expense Transactions.";

        Params[nameof(GetAllExpenseTransactionsEndpoint.RequestModel.WorkspaceId)] = 
            "Workspace ID (simple GUID, user should have access to it).";
        
        Response(
            (int)HttpStatusCode.OK,
            "OK response with body.",
            example: new[]
            {
                new ExpenseTransactionModel(
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
                    ["uber", "taxi"]),
                new ExpenseTransactionModel(
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
                    ["bolt", "taxi"])
            });
        
        Response(
            (int)HttpStatusCode.BadRequest,
            "Bad Request with Error.",
            example: new Error(
                "Validation.Unspecified",
                "Unspecified validation error occured. Check your input and try again."));
    }
}