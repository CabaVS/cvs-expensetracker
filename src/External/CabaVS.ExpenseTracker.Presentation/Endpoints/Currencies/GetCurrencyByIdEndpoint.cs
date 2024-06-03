using CabaVS.ExpenseTracker.Application.Features.Currencies.Queries;
using CabaVS.ExpenseTracker.Presentation.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CabaVS.ExpenseTracker.Presentation.Endpoints.Currencies;

internal sealed class GetCurrencyByIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/currencies/{id:guid}", async (
                Guid id,
                ISender sender,
                CancellationToken ct) =>
            {
                var query = new GetCurrencyByIdQuery(id);

                var result = await sender.Send(query, ct);

                return result.ToDefaultApiResponse();
            })
            .WithName(nameof(GetCurrencyByIdEndpoint))
            .WithTags(EndpointTags.Currencies);
    }
}