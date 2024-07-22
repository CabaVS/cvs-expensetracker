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

internal sealed class UpdateIncomeTransactionEndpoint(ISender sender)
    : Endpoint<UpdateIncomeTransactionEndpoint.RequestModel, Results<Ok, BadRequest<Error>>>
{
    public override void Configure()
    {
        Put("api/workspaces/{workspaceId:guid}/income-transactions/{incomeTransactionId:guid}");
        Options(x =>
        {
            x.WithName(nameof(UpdateIncomeTransactionEndpoint));
            x.WithTags(EndpointTags.IncomeTransactions);
        });
    }

    public override async Task<Results<Ok, BadRequest<Error>>> ExecuteAsync(RequestModel req, CancellationToken ct)
    {
        var command = new UpdateIncomeTransactionCommand(
            req.WorkspaceId,
            req.IncomeTransactionId,
            req.Date,
            req.IncomeCategoryId,
            req.AmountInIncomeCategoryCurrency,
            req.BalanceId,
            req.AmountInBalanceCurrency,
            req.Tags);

        var result = await sender.Send(command, ct);

        return result.ToDefaultApiResponse();
    }

    public sealed record RequestModel(
        Guid WorkspaceId,
        Guid IncomeTransactionId,
        DateOnly Date,
        Guid IncomeCategoryId,
        Guid BalanceId,
        decimal AmountInIncomeCategoryCurrency,
        decimal AmountInBalanceCurrency,
        IEnumerable<string> Tags);
}

internal sealed class UpdateIncomeTransactionEndpointSummary : Summary<UpdateIncomeTransactionEndpoint>
{
    public UpdateIncomeTransactionEndpointSummary()
    {
        Summary = "Update an Income Transaction.";
        Description = "Updates an existing Income Transaction.";
        
        Params[nameof(UpdateIncomeTransactionEndpoint.RequestModel.WorkspaceId)] = 
            "Workspace ID (simple GUID, user should have access to it).";
        
        Params[nameof(UpdateIncomeTransactionEndpoint.RequestModel.IncomeTransactionId)] = 
            "Income Transaction ID to search by (simple GUID).";

        ExampleRequest =
            new UpdateIncomeTransactionEndpoint.RequestModel(
                Guid.Empty,
                Guid.Empty,
                new DateOnly(2020, 10, 20),
                new Guid("03EE7A34-4937-4490-AC3D-EB6E1154F22F"),
                new Guid("9F771779-DBDA-4130-86BD-6E94A28C5649"),
                1234.56m,
                1234.56m,
                ["taxi", "uber"]);
        
        Response(
            (int)HttpStatusCode.OK,
            "OK response without body.");
        
        Response(
            (int)HttpStatusCode.BadRequest,
            "Bad Request with Error.",
            example: new Error(
                "Validation.Unspecified",
                "Unspecified validation error occured. Check your input and try again."));
    }
}