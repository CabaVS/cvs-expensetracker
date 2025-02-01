using CabaVS.ExpenseTracker.Application.Features.Transactions.Models;
using CabaVS.ExpenseTracker.Application.Features.Transactions.Queries;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Presentation.Extensions;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CabaVS.ExpenseTracker.Presentation.Endpoints.Transactions;

internal sealed class GetAllTransactionsEndpoint(ISender sender) : Endpoint<
    GetAllTransactionsEndpoint.RequestModel,
    Results<Ok<TransactionModel[]>, BadRequest<Error>>>
{
    public override void Configure()
    {
        Get("api/workspaces/{workspaceId:guid}/transactions");
        Options(x =>
        {
            x.WithName(nameof(GetAllTransactionsEndpoint));
            x.WithTags(EndpointTags.Transactions);
        });
    }

    public override async Task<Results<Ok<TransactionModel[]>, BadRequest<Error>>> ExecuteAsync(
        RequestModel req, CancellationToken ct)
    {
        var query = new GetAllTransactionsQuery(req.WorkspaceId);

        Result<TransactionModel[]> result = await sender.Send(query, ct);

        return result.ToDefaultApiResponse();
    }

    internal sealed record RequestModel(Guid WorkspaceId);
}
