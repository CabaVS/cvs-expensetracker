using CabaVS.ExpenseTracker.Application.Features.Transactions.Commands;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Presentation.Extensions;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CabaVS.ExpenseTracker.Presentation.Endpoints.Transactions;

internal sealed class DeleteTransactionEndpoint(ISender sender) : Endpoint<
    DeleteTransactionEndpoint.RequestModel,
    Results<Ok, BadRequest<Error>>>
{
    public override void Configure()
    {
        Delete("api/workspaces/{workspaceId:guid}/transactions/{transactionId:guid}");
        Options(x =>
        {
            x.WithName(nameof(DeleteTransactionEndpoint));
            x.WithTags(EndpointTags.Transactions);
        });
    }

    public override async Task<Results<Ok, BadRequest<Error>>> ExecuteAsync(
        RequestModel req, CancellationToken ct)
    {
        var query = new DeleteTransactionCommand(req.TransactionId, req.WorkspaceId);

        Result result = await sender.Send(query, ct);

        return result.ToDefaultApiResponse();
    }

    internal sealed record RequestModel(Guid TransactionId, Guid WorkspaceId);
}
