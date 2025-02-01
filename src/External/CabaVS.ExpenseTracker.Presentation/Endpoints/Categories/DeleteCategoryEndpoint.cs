using CabaVS.ExpenseTracker.Application.Features.Categories.Commands;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Presentation.Extensions;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CabaVS.ExpenseTracker.Presentation.Endpoints.Categories;

internal sealed class DeleteCategoryEndpoint(ISender sender)
    : Endpoint<
        DeleteCategoryEndpoint.RequestModel,
        Results<Ok, BadRequest<Error>>>
{
    public override void Configure()
    {
        Delete("api/workspaces/{workspaceId:guid}/categories/{categoryId:guid}");
        Options(x =>
        {
            x.WithName(nameof(DeleteCategoryEndpoint));
            x.WithTags(EndpointTags.Categories);
        });
    }

    public override async Task<Results<Ok, BadRequest<Error>>> ExecuteAsync(
        RequestModel req, CancellationToken ct)
    {
        var query = new DeleteCategoryCommand(req.WorkspaceId, req.CategoryId);
        
        Result result = await sender.Send(query, ct);
        
        return result.ToDefaultApiResponse();
    }

    public sealed record RequestModel(Guid WorkspaceId, Guid CategoryId);
}
