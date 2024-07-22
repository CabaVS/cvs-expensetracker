using System.Net;
using CabaVS.ExpenseTracker.Application.Features.Transactions.Commands;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Presentation.Extensions;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CabaVS.ExpenseTracker.Presentation.Endpoints.IncomeTransactions;

internal sealed class CreateIncomeTransactionEndpoint(ISender sender) : Endpoint<
    CreateIncomeTransactionEndpoint.RequestModel,
    Results<CreatedAtRoute, BadRequest<Error>>>
{
    public override void Configure()
    {
        Post("api/workspaces/{workspaceId:guid}/income-transactions");
        Options(x =>
        {
            x.WithName(nameof(CreateIncomeTransactionEndpoint));
            x.WithTags(EndpointTags.IncomeTransactions);
        });
    }

    public override async Task<Results<CreatedAtRoute, BadRequest<Error>>> ExecuteAsync(RequestModel req, CancellationToken ct)
    {
        var command = new CreateIncomeTransactionCommand(
            req.WorkspaceId,
            req.Date,
            req.IncomeCategoryId,
            req.AmountInIncomeCategoryCurrency,
            req.BalanceId,
            req.AmountInBalanceCurrency,
            req.Tags);

        var result = await sender.Send(command, ct);

        return result.ToDefaultApiResponse(
            nameof(GetIncomeTransactionByIdEndpoint),
            id => new { req.WorkspaceId, IncomeTransactionId = id });
    }
    
    public sealed record RequestModel(
        Guid WorkspaceId, DateOnly Date, 
        Guid BalanceId, decimal AmountInBalanceCurrency, 
        Guid IncomeCategoryId, decimal AmountInIncomeCategoryCurrency,
        IEnumerable<string> Tags);
}

internal sealed class CreateIncomeTransactionEndpointSummary : Summary<CreateIncomeTransactionEndpoint>
{
    public CreateIncomeTransactionEndpointSummary()
    {
        Summary = "Create an Income Transaction.";
        Description = "Creates a new Income Transaction.";
        
        Params[nameof(CreateIncomeTransactionEndpoint.RequestModel.WorkspaceId)] = 
            "Workspace ID (simple GUID, user should have access to it).";

        ExampleRequest =
            new CreateIncomeTransactionEndpoint.RequestModel(
                Guid.Empty, new DateOnly(2020, 10, 20),
                new Guid("4174BD42-2D83-417B-B2DD-48802C427040"), 1234.56m,
                new Guid("8326BEC6-7BAD-47CA-9F10-79AD9F109753"), 4567.89m,
                ["salary"]);
        
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