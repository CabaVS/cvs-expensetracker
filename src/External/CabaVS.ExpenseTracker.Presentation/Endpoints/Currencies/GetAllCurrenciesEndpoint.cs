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

internal sealed class GetAllCurrenciesEndpoint(ISender sender)
    : EndpointWithoutRequest<
        Results<Ok<GetAllCurrenciesEndpoint.ResponseModel>, BadRequest<Error>>>
{
    public override void Configure()
    {
        Get("/api/currencies");
        Options(x =>
        {
            x.WithName(nameof(GetAllCurrenciesEndpoint));
            x.WithTags(EndpointTags.Currencies);
        });
    }

    public override async Task<Results<Ok<ResponseModel>, BadRequest<Error>>> ExecuteAsync(CancellationToken ct)
    {
        var query = new GetAllCurrenciesQuery();
        
        Result<CurrencyModel[]> result = await sender.Send(query, ct);

        return result.ToDefaultApiResponse(currencies => new ResponseModel(currencies));
    }

    internal sealed record ResponseModel(CurrencyModel[] Currencies);
    
    internal sealed class EndpointSummary : Summary<GetAllCurrenciesEndpoint>
    {
        public EndpointSummary()
        {
            Summary = "Get all Currencies";
            Description = "Get all Currencies existing in the system.";
        
            Response(
                (int)HttpStatusCode.OK,
                "OK response with body.",
                example: new ResponseModel(
                [
                    new CurrencyModel(
                        Guid.NewGuid(),
                        "United States Dollar",
                        "USD",
                        "$"),
                    new CurrencyModel(
                        Guid.NewGuid(),
                        "Euro",
                        "EUR",
                        "€")
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
