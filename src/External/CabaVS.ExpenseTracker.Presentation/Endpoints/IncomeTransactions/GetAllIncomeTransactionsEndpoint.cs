using System.Net;
using CabaVS.ExpenseTracker.Application.Features.Balances.Models;
using CabaVS.ExpenseTracker.Application.Features.Categories.Models;
using CabaVS.ExpenseTracker.Application.Features.Currencies.Models;
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

internal sealed class GetAllIncomeTransactionsEndpoint(ISender sender) 
    : Endpoint<
        GetAllIncomeTransactionsEndpoint.RequestModel,
        Results<Ok<IncomeTransactionModel[]>, BadRequest<Error>>>
{
    public override void Configure()
    {
        AllowAnonymous();
        Get("api/workspaces/{workspaceId:guid}/income-transactions");
        Options(x =>
        {
            x.WithName(nameof(GetAllIncomeTransactionsEndpoint));
            x.WithTags(EndpointTags.IncomeTransactions);
        });
    }

    public override async Task<Results<Ok<IncomeTransactionModel[]>, BadRequest<Error>>> ExecuteAsync(
        RequestModel req,
        CancellationToken ct)
    {
        var query = new GetAllIncomeTransactionsQuery(req.WorkspaceId);

        var result = await sender.Send(query, ct);

        return result.ToDefaultApiResponse();
    }

    internal sealed record RequestModel(Guid WorkspaceId);
}

internal sealed class GetAllIncomeTransactionsEndpointSummary : Summary<GetAllIncomeTransactionsEndpoint>
{
    public GetAllIncomeTransactionsEndpointSummary()
    {
        Summary = "Get all Income Transactions";
        Description = "Gets all Income Transactions.";

        Params[nameof(GetAllIncomeTransactionsEndpoint.RequestModel.WorkspaceId)] = 
            "Workspace ID (simple GUID, user should have access to it).";
        
        Response(
            (int)HttpStatusCode.OK,
            "OK response with body.",
            example: new[]
            {
                new IncomeTransactionModel(
                new Guid("0D64417A-AC39-44C5-857A-01BECE08700D"),
                new DateOnly(2020, 10, 20),
                1234.56m,
                1234.56m)
            {
                Source = new IncomeCategoryModel(
                    new Guid("0D64417A-AC39-44C5-857A-01BECE08700D"),
                    "My Company")
                {
                    Currency = new CurrencyModel(
                        new Guid("AE6320FD-AE15-44AE-B65D-18C6541042DD"),
                        "United States Dollar",
                        "USD",
                        "$")
                },
                Destination = new BalanceModel(
                    new Guid("E650E18C-0069-45B3-B56A-AB8C3BA8D8AA"),
                    "Card USD",
                    1234.56m)
                {
                    Currency = new CurrencyModel(
                        new Guid("AE6320FD-AE15-44AE-B65D-18C6541042DD"),
                        "United States Dollar",
                        "USD",
                        "$")
                }
            },
            new IncomeTransactionModel(
                new Guid("0D64417A-AC39-44C5-857A-01BECE08700D"),
                new DateOnly(2020, 10, 20),
                1234.56m,
                1234.56m)
            {
                Source = new IncomeCategoryModel(
                    new Guid("0D64417A-AC39-44C5-857A-01BECE08700D"),
                    "My Company #2")
                {
                    Currency = new CurrencyModel(
                        new Guid("AE6320FD-AE15-44AE-B65D-18C6541042DD"),
                        "United States Dollar",
                        "USD",
                        "$")
                },
                Destination = new BalanceModel(
                    new Guid("E650E18C-0069-45B3-B56A-AB8C3BA8D8AA"),
                    "Card USD",
                    1234.56m)
                {
                    Currency = new CurrencyModel(
                        new Guid("AE6320FD-AE15-44AE-B65D-18C6541042DD"),
                        "United States Dollar",
                        "USD",
                        "$")
                }
            }
            });
        
        Response(
            (int)HttpStatusCode.BadRequest,
            "Bad Request with Error.",
            example: new Error(
                "Validation.Unspecified",
                "Unspecified validation error occured. Check your input and try again."));
    }
}