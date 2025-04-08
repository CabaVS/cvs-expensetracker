using System.Net;
using CabaVS.ExpenseTracker.Application.Features.Transactions.Commands;
using CabaVS.ExpenseTracker.Domain.Enums;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Presentation.Extensions;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CabaVS.ExpenseTracker.Presentation.Endpoints.Transactions;

internal sealed class CreateTransactionEndpoint(ISender sender)
    : Endpoint<
        CreateTransactionEndpoint.RequestModel,
        Results<CreatedAtRoute, BadRequest<Error>>>
{
    public override void Configure()
    {
        Post("/api/workspaces/{workspaceId}/transactions");
        Options(x =>
        {
            x.WithName(nameof(CreateTransactionEndpoint));
            x.WithTags(EndpointTags.Transactions);
        });
    }

    public override async Task<Results<CreatedAtRoute, BadRequest<Error>>> ExecuteAsync(RequestModel req, CancellationToken ct)
    {
        var command = new CreateTransactionCommand(
            req.WorkspaceId,
            req.Date,
            req.Type,
            req.Tags,
            req.AmountInSourceCurrency,
            req.AmountInDestinationCurrency,
            req.SourceId,
            req.DestinationId);

        Result<Guid> result = await sender.Send(command, ct);
        
        return result.ToDefaultApiResponse(
            nameof(GetSingleTransactionByIdEndpoint),
            id => new
            {
                req.WorkspaceId,
                TransactionId = id
            });
    }

    internal sealed record RequestModel(Guid WorkspaceId,
        DateOnly Date, TransactionType Type, IEnumerable<string> Tags,
        decimal AmountInSourceCurrency, decimal AmountInDestinationCurrency,
        Guid SourceId, Guid DestinationId);

    internal sealed class EndpointSummary : Summary<CreateTransactionEndpoint>
    {
        public EndpointSummary()
        {
            Summary = "Create a Transaction";
            Description = "Creates a Transaction within specified workspace.";
            
            Params[nameof(RequestModel.WorkspaceId)] = "Workspace ID to verify access.";

            ExampleRequest =
                new RequestModel(
                    Guid.Empty,
                    new DateOnly(2020, 10, 20),
                    TransactionType.Expense,
                    ["subscription", "open-ai"],
                    1234.56m,
                    1234.56m,
                    Guid.NewGuid(),
                    Guid.NewGuid());
        
            Response(
                (int)HttpStatusCode.Created,
                "Created At Route response without a body. Location header is filled with Id of created entity.");
        
            Response(
                (int)HttpStatusCode.BadRequest,
                "Bad Request with Error.",
                example: new Error(
                    "Validation.Unspecified",
                    "Unspecified validation error occured. Check your input and try again."));
        }
    }
}
