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

internal sealed class GetAllBalancesEndpoint(ISender sender) 
    : Endpoint<
        GetAllBalancesEndpoint.GetAllBalancesEndpointRequest,
        Results<Ok<BalanceModel[]>, BadRequest<Error>>>
{
    public override void Configure()
    {
        Get("api/workspaces/{workspaceId:guid}/balances");
        Options(x =>
        {
            x.WithName(nameof(GetAllBalancesEndpoint));
            x.WithTags(EndpointTags.Balances);
        });
    }

    public override async Task<Results<Ok<BalanceModel[]>, BadRequest<Error>>> ExecuteAsync(
        GetAllBalancesEndpointRequest req,
        CancellationToken ct)
    {
        var query = new GetAllBalancesQuery(req.WorkspaceId);

        Result<BalanceModel[]> result = await sender.Send(query, ct);

        return result.ToDefaultApiResponse();
    }

    internal sealed record GetAllBalancesEndpointRequest(Guid WorkspaceId);
}

internal sealed class GetAllBalancesEndpointSummary : Summary<GetAllBalancesEndpoint>
{
    public GetAllBalancesEndpointSummary()
    {
        Summary = "Get all Balances";
        Description = "Gets all Balances. Ensures that User is a member of the parent Workspace.";

        Params[nameof(GetBalanceByIdEndpoint.GetBalanceByIdEndpointRequest.WorkspaceId)] = 
            "Workspace ID to verify access.";
        
        Response(
            (int)HttpStatusCode.OK,
            "OK response with body.",
            example: new BalanceModel[]
            {
                new(
                    new Guid("DCF2732A-E43A-4CB1-8718-EA1AD3543BA1"),
                    "Universal Bank Account",
                    1234.56m,
                    new CurrencyModel(
                        new Guid("9BC67337-3BB5-469E-A71C-B995305F379B"),
                        "United States dollar",
                        "USD",
                        "$")),
                new(
                    new Guid("DCF2732A-E43A-4CB1-8718-EA1AD3543BA1"),
                    "BNP Bank Account",
                    -12.34m,
                    new CurrencyModel(
                        new Guid("9BC67337-3BB5-469E-A71C-B995305F379B"),
                        "United States dollar",
                        "USD",
                        "$"))
            });
        
        Response(
            (int)HttpStatusCode.BadRequest,
            "Bad Request with Error.",
            example: new Error(
                "Validation.Unspecified",
                "Unspecified validation error occured. Check your input and try again."));
    }
}
