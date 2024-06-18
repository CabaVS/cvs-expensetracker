using System.Net;
using CabaVS.ExpenseTracker.Application.Features.Transactions.Commands;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Presentation.Extensions;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CabaVS.ExpenseTracker.Presentation.Endpoints.ExpenseTransactions;

internal sealed class CreateExpenseTransactionEndpoint(ISender sender) : Endpoint<
    CreateExpenseTransactionEndpoint.RequestModel,
    Results<CreatedAtRoute, BadRequest<Error>>>
{
    public override void Configure()
    {
        AllowAnonymous();
        Post("api/workspaces/{workspaceId:guid}/expense-transactions");
        Options(x =>
        {
            x.WithName(nameof(CreateExpenseTransactionEndpoint));
            x.WithTags(EndpointTags.ExpenseTransactions);
        });
    }

    public override async Task<Results<CreatedAtRoute, BadRequest<Error>>> ExecuteAsync(RequestModel req, CancellationToken ct)
    {
        var command = new CreateExpenseTransactionCommand(
            req.WorkspaceId,
            req.Date,
            req.ExpenseCategoryId,
            req.AmountInExpenseCategoryCurrency,
            req.BalanceId,
            req.AmountInBalanceCurrency);

        var result = await sender.Send(command, ct);

        return result.ToDefaultApiResponse(
            nameof(GetExpenseTransactionByIdEndpoint),
            id => new { req.WorkspaceId, ExpenseTransactionId = id });
    }
    
    public sealed record RequestModel(
        Guid WorkspaceId, DateOnly Date, 
        Guid BalanceId, decimal AmountInBalanceCurrency, 
        Guid ExpenseCategoryId, decimal AmountInExpenseCategoryCurrency);
}

internal sealed class CreateExpenseTransactionEndpointSummary : Summary<CreateExpenseTransactionEndpoint>
{
    public CreateExpenseTransactionEndpointSummary()
    {
        Summary = "Create an Expense Transaction.";
        Description = "Creates a new Expense Transaction.";
        
        Params[nameof(CreateExpenseTransactionEndpoint.RequestModel.WorkspaceId)] = 
            "Workspace ID (simple GUID, user should have access to it).";

        ExampleRequest =
            new CreateExpenseTransactionEndpoint.RequestModel(
                Guid.Empty, new DateOnly(2020, 10, 20),
                new Guid("4174BD42-2D83-417B-B2DD-48802C427040"), 1234.56m,
                new Guid("8326BEC6-7BAD-47CA-9F10-79AD9F109753"), 4567.89m);
        
        Response(
            (int)HttpStatusCode.Created,
            "Created At response with Location header filled.");
        
        Response(
            (int)HttpStatusCode.BadRequest,
            "Bad Request with Error.",
            example: new Error(
                "Validation.Unspecified",
                "Unspecified validation error occured. Check your input and try again."));
    }
}