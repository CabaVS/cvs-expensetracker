using System.Net;
using CabaVS.ExpenseTracker.Application.Features.Balances.Models;
using CabaVS.ExpenseTracker.Application.Features.Balances.Queries;
using CabaVS.ExpenseTracker.Application.Features.Currencies.Models;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Presentation.Extensions;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CabaVS.ExpenseTracker.Presentation.Endpoints.Balances;

internal sealed class GetBalanceByIdEndpoint(ISender sender) : Endpoint<
    GetBalanceByIdEndpoint.RequestModel,
    Results<Ok<GetBalanceByIdEndpoint.ResponseModel>, BadRequest<Error>>>
{
    public override void Configure()
    {
        Get("/api/workspaces/{workspaceId:guid}/balances/{balanceId:guid}");
        Options(x =>
        {
            x.WithName(nameof(GetBalanceByIdEndpoint));
            x.WithTags(EndpointTags.Balances);
        });
    }

    public override async Task<Results<Ok<ResponseModel>, BadRequest<Error>>> ExecuteAsync(
        RequestModel req, CancellationToken ct)
    {
        var query = new GetBalanceByIdQuery(req.WorkspaceId, req.BalanceId);
        
        Result<BalanceModel> result = await sender.Send(query, ct);

        return result.ToDefaultApiResponse(balance => new ResponseModel(balance));
    }

    internal sealed record RequestModel(Guid WorkspaceId, Guid BalanceId);
    internal sealed record ResponseModel(BalanceModel BalanceDetails);
    
    internal sealed class EndpointSummary : Summary<GetBalanceByIdEndpoint>
    {
        public EndpointSummary()
        {
            Summary = "Get a Balance by ID";
            Description = "Get a Balance by ID within specified workspace.";
        
            Response(
                (int)HttpStatusCode.OK,
                "OK response with body.",
                example: new ResponseModel(
                    new BalanceModel(
                        Guid.NewGuid(),
                        "My USD card",
                        1000.50m,
                        new CurrencySlimModel(
                            Guid.NewGuid(),
                            "USD"))));
        
            Response(
                (int)HttpStatusCode.BadRequest,
                "Bad Request with Error.",
                example: new Error(
                    "Validation.Unspecified",
                    "Unspecified validation error occured. Check your input and try again."));
        }
    }
}
