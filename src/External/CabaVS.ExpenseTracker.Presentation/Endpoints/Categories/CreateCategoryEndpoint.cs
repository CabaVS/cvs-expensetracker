using CabaVS.ExpenseTracker.Application.Features.Categories.Commands;
using CabaVS.ExpenseTracker.Domain.Enums;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Presentation.Extensions;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CabaVS.ExpenseTracker.Presentation.Endpoints.Categories;

internal sealed class CreateCategoryEndpoint(ISender sender)
    : Endpoint<
        CreateCategoryEndpoint.RequestModel,
        Results<CreatedAtRoute, BadRequest<Error>>>
{
    public override void Configure()
    {
        Post("api/workspaces/{workspaceId:guid}/categories");
        Options(x =>
        {
            x.WithName(nameof(CreateCategoryEndpoint));
            x.WithTags(EndpointTags.Categories);
        });
    }

    public override async Task<Results<CreatedAtRoute, BadRequest<Error>>> ExecuteAsync(
        RequestModel req, CancellationToken ct)
    {
        var query = new CreateCategoryCommand(
            req.WorkspaceId, req.CurrencyId, req.Name, req.Type);
        
        Result<Guid> result = await sender.Send(query, ct);
        
        return result.ToDefaultApiResponse(
            nameof(GetCategoryByIdEndpoint),
            id => new { CategoryId = id });
    }

    public sealed record RequestModel(Guid WorkspaceId, Guid CurrencyId, string Name, CategoryType Type);
}
