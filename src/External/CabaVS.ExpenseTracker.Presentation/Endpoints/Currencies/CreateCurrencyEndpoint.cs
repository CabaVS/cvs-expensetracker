using CabaVS.ExpenseTracker.Application.Features.Currencies.Commands;
using CabaVS.ExpenseTracker.Presentation.Extensions;
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

                return result.ToDefaultApiResponse(nameof(GetCurrencyByIdEndpoint));
            })
            .WithName(nameof(CreateCurrencyEndpoint))
            .WithTags(EndpointTags.Currencies);
    }

    private sealed record CreateCurrencyRequestModel(string Name, string Code, string Symbol);
}