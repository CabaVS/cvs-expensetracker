using System.Net;
using CabaVS.ExpenseTracker.Application.Features.Currencies.Models;
using CabaVS.ExpenseTracker.Application.Features.Currencies.Queries;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Presentation.Extensions;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CabaVS.ExpenseTracker.Presentation.Endpoints.Currencies;

internal sealed class GetCurrenciesEndpoint(ISender sender) 
    : EndpointWithoutRequest<
        Results<Ok<CurrencyModel[]>, BadRequest<Error>>>
{
    public override void Configure()
    {
        Get("api/currencies");
        Options(x =>
        {
            x.WithName(nameof(GetCurrenciesEndpoint));
            x.WithTags(EndpointTags.Currencies);
        });
    }

    public override async Task<Results<Ok<CurrencyModel[]>, BadRequest<Error>>> ExecuteAsync(
        CancellationToken ct)
    {
        var query = new GetCurrenciesQuery();

        Result<CurrencyModel[]> result = await sender.Send(query, ct);

        return result.ToDefaultApiResponse();
    }
}

internal sealed class GetCurrenciesEndpointSummary : Summary<GetCurrenciesEndpoint>
{
    public GetCurrenciesEndpointSummary()
    {
        Summary = "Get Currencies";
        Description = "Gets a collection of existing Currencies.";
        
        Response(
            (int)HttpStatusCode.OK,
            "OK response with body.",
            example: new CurrencyModel[]
            {
                new(
                    new Guid("9BC67337-3BB5-469E-A71C-B995305F379B"),
                    "United States dollar",
                    "USD",
                    "$"),
                new(
                    new Guid("02378D78-B79B-41E7-B236-501089F19F63"),
                    "Euro",
                    "EUR",
                    "\u20ac")
            });
        
        Response(
            (int)HttpStatusCode.BadRequest,
            "Bad Request with Error.",
            example: new Error(
                "Validation.Unspecified",
                "Unspecified validation error occured. Check your input and try again."));
    }
}
