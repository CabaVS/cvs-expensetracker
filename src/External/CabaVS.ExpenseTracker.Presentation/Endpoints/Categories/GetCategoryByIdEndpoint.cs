using CabaVS.ExpenseTracker.Application.Features.Categories.Models;
using CabaVS.ExpenseTracker.Application.Features.Categories.Queries;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Presentation.Extensions;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CabaVS.ExpenseTracker.Presentation.Endpoints.Categories;

internal sealed class GetCategoryByIdEndpoint(ISender sender)
    : Endpoint<
        GetCategoryByIdEndpoint.RequestModel,
        Results<Ok<CategoryModel>, BadRequest<Error>>>
{
    public override void Configure()
    {
        Get("api/workspaces/{workspaceId:guid}/categories/{categoryId:guid}");
        Options(x =>
        {
            x.WithName(nameof(GetCategoryByIdEndpoint));
            x.WithTags(EndpointTags.Categories);
        });
    }

    public override async Task<Results<Ok<CategoryModel>, BadRequest<Error>>> ExecuteAsync(
        RequestModel req, CancellationToken ct)
    {
        var query = new GetCategoryByIdQuery(req.CategoryId, req.WorkspaceId);
        
        Result<CategoryModel> result = await sender.Send(query, ct);
        
        return result.ToDefaultApiResponse();
    }

    public sealed record RequestModel(Guid WorkspaceId, Guid CategoryId);
}
