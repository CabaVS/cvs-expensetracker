using CabaVS.ExpenseTracker.Application.Features.Currencies.Commands;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace CabaVS.ExpenseTracker.Presentation.Endpoints.Currencies;

internal sealed class CreateCurrencyEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/currencies", async (
                [FromBody] CreateCurrencyRequestModel requestModel,
                ISender sender,
                CancellationToken ct) =>
            {
                var command = new CreateCurrencyCommand(requestModel.Name, requestModel.Code, requestModel.Symbol);

                var result = await sender.Send(command, ct);

                return result.IsSuccess
                    ? Results.Ok(result.Value) // TODO: Should return 201 response code
                    : Results.BadRequest(result.Error);
            })
            .WithName(nameof(CreateCurrencyEndpoint))
            .WithTags(EndpointTags.Currencies);
    }

    private sealed record CreateCurrencyRequestModel(string Name, string Code, string Symbol);
}