using System.Diagnostics;
using System.Net;
using CabaVS.ExpenseTracker.Application.Features.Currencies.Models;
using CabaVS.ExpenseTracker.Application.Features.Currencies.Queries;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CabaVS.ExpenseTracker.Presentation.Endpoints.Currencies;

internal sealed class GetAllCurrenciesEndpoint(ISender sender) : EndpointWithoutRequest<Ok<CurrencyModel[]>>
{
    public override void Configure()
    {
        AllowAnonymous();
        Get("api/currencies");
        Options(x => x.WithName(nameof(GetAllCurrenciesEndpoint)));
    }

    public override async Task<Ok<CurrencyModel[]>> ExecuteAsync(CancellationToken ct)
    {
        var query = new GetAllCurrenciesQuery();

        var result = await sender.Send(query, ct);
        if (result.IsFailure)
        {
            throw new UnreachableException();
        }

        return TypedResults.Ok(result.Value);
    }
}

internal sealed class GetAllCurrenciesEndpointSummary : Summary<GetAllCurrenciesEndpoint>
{
    public GetAllCurrenciesEndpointSummary()
    {
        Summary = "Get all Currencies";
        Description = "Gets all Currencies.";
        
        Response(
            (int)HttpStatusCode.OK,
            "OK response with body.",
            example: new[]
            {
                new CurrencyModel(
                    new Guid("AE6320FD-AE15-44AE-B65D-18C6541042DD"),
                    "United States Dollar",
                    "USD",
                    "$"),
                new CurrencyModel(
                    new Guid("8DC3B3C7-E1C6-4192-87A6-4A67192ADB94"),
                    "Euro",
                    "EUR",
                    "€")
            });
    }
}