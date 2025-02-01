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

internal sealed class CreateTransactionEndpoint(ISender sender) : Endpoint<
    CreateTransactionEndpoint.RequestModel,
    Results<CreatedAtRoute, BadRequest<Error>>>
{
    public override void Configure()
    {
        Post("api/workspaces/{workspaceId:guid}/transactions");
        Options(x =>
        {
            x.WithName(nameof(CreateTransactionEndpoint));
            x.WithTags(EndpointTags.Transactions);
        });
    }

    public override async Task<Results<CreatedAtRoute, BadRequest<Error>>> ExecuteAsync(
        RequestModel req, CancellationToken ct)
    {
        var query = new CreateTransactionCommand(req.WorkspaceId,
            req.Date, req.Tags, req.AmountInSourceCurrency, req.AmountInDestinationCurrency,
            req.Type, req.SourceId, req.DestinationId);

        Result<Guid> result = await sender.Send(query, ct);

        return result.ToDefaultApiResponse(
            nameof(GetTransactionByIdEndpoint),
            id => new { TransactionId = id });
    }

    internal sealed record RequestModel(Guid WorkspaceId,
        DateOnly Date, string[] Tags, decimal AmountInSourceCurrency, decimal AmountInDestinationCurrency,
        TransactionType Type, Guid SourceId, Guid DestinationId);
}
