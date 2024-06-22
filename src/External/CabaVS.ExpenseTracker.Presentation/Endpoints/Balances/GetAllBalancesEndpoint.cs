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

internal sealed class GetAllBalancesEndpoint(ISender sender) : Endpoint<
    GetAllBalancesEndpoint.RequestModel,
    Results<Ok<BalanceModel[]>, BadRequest<Error>>>
{
    public override void Configure()
    {
        AllowAnonymous();
        Get("api/workspaces/{workspaceId:guid}/balances");
        Options(x =>
        {
            x.WithName(nameof(GetAllBalancesEndpoint));
            x.WithTags(EndpointTags.Balances);
        });
    }

    public override async Task<Results<Ok<BalanceModel[]>, BadRequest<Error>>> ExecuteAsync(RequestModel req, CancellationToken ct)
    {
        var query = new GetAllBalancesQuery(req.WorkspaceId);

        var result = await sender.Send(query, ct);

        return result.ToDefaultApiResponse();
    }
    
    internal sealed record RequestModel(Guid WorkspaceId);
}

internal sealed class GetAllBalancesEndpointSummary : Summary<GetAllBalancesEndpoint>
{
    public GetAllBalancesEndpointSummary()
    {
        Summary = "Get all Balances";
        Description = "Gets all Balances.";
        
        Params[nameof(GetAllBalancesEndpoint.RequestModel.WorkspaceId)] = 
            "Workspace ID (simple GUID, user should have access to it).";
        
        Response(
            (int)HttpStatusCode.OK,
            "OK response with body.",
            example: new[]
            {
                new BalanceModel(
                    new Guid("19790A33-C18F-4F21-B52A-FB73FCAB6C46"),
                    "Card USD",
                    1234.56m,
                    new CurrencyModel(
                        new Guid("AE6320FD-AE15-44AE-B65D-18C6541042DD"),
                        "United States Dollar",
                        "USD",
                        "$")),
                new BalanceModel(
                    new Guid("1529F891-319B-4DFF-8943-A25750C8BCB3"),
                    "Cash EUR",
                    789.00m,
                    new CurrencyModel(
                        new Guid("8DC3B3C7-E1C6-4192-87A6-4A67192ADB94"),
                        "Euro",
                        "EUR",
                        "€"))
            });
        
        Response(
            (int)HttpStatusCode.BadRequest,
            "Bad Request with Error.",
            example: new Error(
                "Validation.Unspecified",
                "Unspecified validation error occured. Check your input and try again."));
    }
}