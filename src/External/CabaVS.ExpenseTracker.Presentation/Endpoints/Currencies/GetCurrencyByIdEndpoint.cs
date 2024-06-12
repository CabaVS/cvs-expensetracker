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

internal sealed class GetCurrencyByIdEndpoint(ISender sender) 
    : Endpoint<
        GetCurrencyByIdEndpoint.GetCurrencyByIdEndpointRequest,
        Results<Ok<CurrencyModel>, BadRequest<Error>>>
{
    public override void Configure()
    {
        AllowAnonymous();
        Get("api/currencies/{id:guid}");
        Options(x =>
        {
            x.WithName(nameof(GetCurrencyByIdEndpoint));
            x.WithTags(EndpointTags.Currencies);
        });
    }

    public override async Task<Results<Ok<CurrencyModel>, BadRequest<Error>>> ExecuteAsync(
        GetCurrencyByIdEndpointRequest req,
        CancellationToken ct)
    {
        var query = new GetCurrencyByIdQuery(req.Id);

        var result = await sender.Send(query, ct);

        return result.ToDefaultApiResponse();
    }

    internal sealed record GetCurrencyByIdEndpointRequest(Guid Id);
}

internal sealed class GetCurrencyByIdEndpointSummary : Summary<GetCurrencyByIdEndpoint>
{
    public GetCurrencyByIdEndpointSummary()
    {
        Summary = "Get Currency by ID";
        Description = "Gets a Currency by provided ID.";

        Params[nameof(GetCurrencyByIdEndpoint.GetCurrencyByIdEndpointRequest.Id)] = 
            "Currency ID to search by (simple GUID).";
        
        Response(
            (int)HttpStatusCode.OK,
            "OK response with body.",
            example: new CurrencyModel(
                new Guid("AE6320FD-AE15-44AE-B65D-18C6541042DD"),
                "United States Dollar",
                "USD",
                "$"));
        
        Response(
            (int)HttpStatusCode.BadRequest,
            "Bad Request with Error.",
            example: new Error(
                "Validation.Unspecified",
                "Unspecified validation error occured. Check your input and try again."));
    }
}