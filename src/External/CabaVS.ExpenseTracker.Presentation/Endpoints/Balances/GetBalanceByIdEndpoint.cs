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
        GetBalanceByIdEndpoint.RequestModel,
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
        RequestModel req,
        CancellationToken ct)
    {
        var query = new GetBalanceByIdQuery(req.WorkspaceId, req.BalanceId);

        var result = await sender.Send(query, ct);

        return result.ToDefaultApiResponse();
    }

    internal sealed record RequestModel(Guid WorkspaceId, Guid BalanceId);
}

internal sealed class GetBalanceByIdEndpointSummary : Summary<GetBalanceByIdEndpoint>
{
    public GetBalanceByIdEndpointSummary()
    {
        Summary = "Get Balance by ID";
        Description = "Gets a Balance by provided ID.";

        Params[nameof(GetBalanceByIdEndpoint.RequestModel.WorkspaceId)] = 
            "Workspace ID (simple GUID, user should have access to it).";
        Params[nameof(GetBalanceByIdEndpoint.RequestModel.BalanceId)] = 
            "Balance ID to search by (simple GUID).";
        
        Response(
            (int)HttpStatusCode.OK,
            "OK response with body.",
            example: new BalanceModel(
                new Guid("19790A33-C18F-4F21-B52A-FB73FCAB6C46"),
                "Card USD",
                1234.56m,
                new CurrencyModel(
                    new Guid("AE6320FD-AE15-44AE-B65D-18C6541042DD"),
                    "United States Dollar",
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