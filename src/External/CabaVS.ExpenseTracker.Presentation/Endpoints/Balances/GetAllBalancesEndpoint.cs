using System.Net;
using CabaVS.ExpenseTracker.Application.Features.Balances.Models;
using CabaVS.ExpenseTracker.Application.Features.Balances.Queries;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Presentation.Extensions;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CabaVS.ExpenseTracker.Presentation.Endpoints.Balances;

internal sealed class GetAllBalancesEndpoint(ISender sender) : Endpoint<
    GetAllBalancesEndpoint.RequestModel,
    Results<Ok<GetAllBalancesEndpoint.ResponseModel>, BadRequest<Error>>>
{
    public override void Configure()
    {
        Get("/api/workspaces/{workspaceId:guid}/balances");
        Options(x =>
        {
            x.WithName(nameof(GetAllBalancesEndpoint));
            x.WithTags(EndpointTags.Balances);
        });
    }

    public override async Task<Results<Ok<ResponseModel>, BadRequest<Error>>> ExecuteAsync(
        RequestModel req, CancellationToken ct)
    {
        var query = new GetAllBalancesByWorkspaceIdQuery(req.WorkspaceId);
        
        Result<BalanceModel[]> result = await sender.Send(query, ct);

        return result.ToDefaultApiResponse(balances => new ResponseModel(balances));
    }

    internal sealed record RequestModel(Guid WorkspaceId);
    internal sealed record ResponseModel(BalanceModel[] Balances);
    
    internal sealed class EndpointSummary : Summary<GetAllBalancesEndpoint>
    {
        public EndpointSummary()
        {
            Summary = "Get all Balances";
            Description = "Get all Balances within specified workspace.";
        
            Response(
                (int)HttpStatusCode.OK,
                "OK response with body.",
                example: new ResponseModel(
                [
                    new BalanceModel(
                        Guid.NewGuid(),
                        "My USD card"),
                    new BalanceModel(
                        Guid.NewGuid(),
                        "My PLN card")
                ]));
        
            Response(
                (int)HttpStatusCode.BadRequest,
                "Bad Request with Error.",
                example: new Error(
                    "Validation.Unspecified",
                    "Unspecified validation error occured. Check your input and try again."));
        }
    }
}
