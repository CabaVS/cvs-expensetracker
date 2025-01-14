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

internal sealed class GetBalanceByIdEndpoint(ISender sender) 
    : Endpoint<
        GetBalanceByIdEndpoint.GetBalanceByIdEndpointRequest,
        Results<Ok<BalanceModel>, BadRequest<Error>>>
{
    public override void Configure()
    {
        Get("api/workspaces/{workspaceId:guid}/balances/{balanceId:guid}");
        Options(x =>
        {
            x.WithName(nameof(GetBalanceByIdEndpoint));
            x.WithTags(EndpointTags.Balances);
        });
    }

    public override async Task<Results<Ok<BalanceModel>, BadRequest<Error>>> ExecuteAsync(
        GetBalanceByIdEndpointRequest req,
        CancellationToken ct)
    {
        var query = new GetBalanceByIdQuery(req.BalanceId, req.WorkspaceId);

        Result<BalanceModel> result = await sender.Send(query, ct);

        return result.ToDefaultApiResponse();
    }

    internal sealed record GetBalanceByIdEndpointRequest(Guid WorkspaceId, Guid BalanceId);
}

internal sealed class GetBalanceByIdEndpointSummary : Summary<GetBalanceByIdEndpoint>
{
    public GetBalanceByIdEndpointSummary()
    {
        Summary = "Get Balance by ID";
        Description = "Gets a Balance by provided ID. Ensures that User is a member of the parent Workspace.";

        Params[nameof(GetBalanceByIdEndpoint.GetBalanceByIdEndpointRequest.WorkspaceId)] = 
            "Workspace ID to verify access.";
        Params[nameof(GetBalanceByIdEndpoint.GetBalanceByIdEndpointRequest.BalanceId)] = 
            "Balance ID to search by (simple GUID).";
        
        Response(
            (int)HttpStatusCode.OK,
            "OK response with body.",
            example: new BalanceModel(
                new Guid("DCF2732A-E43A-4CB1-8718-EA1AD3543BA1"),
                "Universal Bank Account",
                1234.56m,
                new CurrencyModel(
                    new Guid("9BC67337-3BB5-469E-A71C-B995305F379B"),
                    "United States dollar",
                    "USD",
                    "$")));
        
        Response(
            (int)HttpStatusCode.BadRequest,
            "Bad Request with Error.",
            example: new Error(
                "Validation.Unspecified",
                "Unspecified validation error occured. Check your input and try again."));
    }
}
