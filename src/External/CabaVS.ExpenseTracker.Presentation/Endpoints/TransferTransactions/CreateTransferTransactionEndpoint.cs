using System.Net;
using CabaVS.ExpenseTracker.Application.Features.TransferTransactions.Commands;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Presentation.Extensions;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CabaVS.ExpenseTracker.Presentation.Endpoints.TransferTransactions;

internal sealed class CreateTransferTransactionEndpoint(ISender sender)
    : Endpoint<
        CreateTransferTransactionEndpoint.RequestModel,
        Results<CreatedAtRoute, BadRequest<Error>>>
{
    public override void Configure()
    {
        Post("api/workspaces/{workspaceId:guid}/transfer-transactions");
        Options(x =>
        {
            x.WithName(nameof(CreateTransferTransactionEndpoint));
            x.WithTags(EndpointTags.TransferTransactions);
        });
    }
    
    public override async Task<Results<CreatedAtRoute, BadRequest<Error>>> ExecuteAsync(
        RequestModel req,
        CancellationToken ct)
    {
        var command = new CreateTransferTransactionCommand(
            req.WorkspaceId,
            req.SourceId,
            req.DestinationId,
            req.CurrencyId,
            req.Amount,
            req.AmountInSourceCurrency,
            req.AmountInDestinationCurrency,
            req.Date,
            req.Tags);

        Result<Guid> result = await sender.Send(command, ct);
        
        return result.ToDefaultApiResponse(
            nameof(GetTransferTransactionByIdEndpoint),
            id => new { req.WorkspaceId, TransferTransactionId = id });
    }
    
    internal sealed record RequestModel(
        Guid WorkspaceId,
        Guid SourceId,
        Guid DestinationId,
        Guid CurrencyId,
        decimal Amount,
        decimal? AmountInSourceCurrency,
        decimal? AmountInDestinationCurrency,
        DateOnly Date,
        string[] Tags);
}

internal sealed class CreateTransferTransactionEndpointSummary : Summary<CreateTransferTransactionEndpoint>
{
    public CreateTransferTransactionEndpointSummary()
    {
        Summary = "Create a Transfer Transaction.";
        Description = "Creates a new Transfer Transaction.";
        
        Params[nameof(GetTransferTransactionByIdEndpoint.GetTransferTransactionByIdEndpointRequest.WorkspaceId)] = 
            "Workspace ID to verify access.";

        ExampleRequest =
            new CreateTransferTransactionEndpoint.RequestModel(
                new Guid("99F4B07F-FCFC-40A4-BAD3-8CAD31EE4A57"),
                new Guid("412B6340-7E1C-48F3-9357-2A6290CAF55E"),
                new Guid("CFFFBD51-1286-4A78-9C28-E11E3E22EE39"),
                new Guid("FAA85644-2B11-4588-94B7-5F63F2E063EE"),
                123.45m,
                678.90m,
                null,
                new DateOnly(2020, 12, 31),
                ["taxi", "uber"]);
        
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
