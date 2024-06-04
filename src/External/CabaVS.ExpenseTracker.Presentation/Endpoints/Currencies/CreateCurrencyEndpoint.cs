using System.Net;
using CabaVS.ExpenseTracker.Application.Features.Currencies.Commands;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Presentation.Extensions;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CabaVS.ExpenseTracker.Presentation.Endpoints.Currencies;

internal sealed class CreateCurrencyEndpoint(ISender sender) 
    : Endpoint<
        CreateCurrencyEndpoint.CreateCurrencyEndpointRequest,
        Results<CreatedAtRoute, BadRequest<Error>>>
{
    public override void Configure()
    {
        AllowAnonymous();
        Post("api/currencies");
        Options(x => x.WithName(nameof(CreateCurrencyEndpoint)));
    }

    public override async Task<Results<CreatedAtRoute, BadRequest<Error>>> ExecuteAsync(
        CreateCurrencyEndpointRequest req,
        CancellationToken ct)
    {
        var command = new CreateCurrencyCommand(
            req.CreateCurrencyModel.Name,
            req.CreateCurrencyModel.Code,
            req.CreateCurrencyModel.Symbol);

        var result = await sender.Send(command, ct);

        return result.ToDefaultApiResponse(nameof(GetCurrencyByIdEndpoint));
    }

    internal sealed record CreateCurrencyEndpointRequest
    {
        [FromBody]
        public CreateCurrencyModel CreateCurrencyModel { get; init; } = default!;
    };
    
    internal sealed record CreateCurrencyModel(string Name, string Code, string Symbol);
}

internal sealed class CreateCurrencyEndpointSummary : Summary<CreateCurrencyEndpoint>
{
    public CreateCurrencyEndpointSummary()
    {
        Summary = "Create a Currency.";
        Description = "Creates a new Currency.";

        ExampleRequest =
            new CreateCurrencyEndpoint.CreateCurrencyModel(
                "United States Dollar", "USD", "$");
        
        Response(
            (int)HttpStatusCode.Created,
            "Created At response with Location header filled.");
        
        Response(
            (int)HttpStatusCode.BadRequest,
            "Bad Request with Error.",
            example: new Error(
                "Validation.Unspecified",
                "Unspecified validation error occured. Check your input and try again."));
    }
}