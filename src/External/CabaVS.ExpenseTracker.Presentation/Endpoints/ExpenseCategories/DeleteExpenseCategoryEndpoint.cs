using CabaVS.ExpenseTracker.Application.Features.Categories.Commands;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Presentation.Extensions;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CabaVS.ExpenseTracker.Presentation.Endpoints.ExpenseCategories;

internal sealed class DeleteExpenseCategoryEndpoint(ISender sender) : Endpoint<
    DeleteExpenseCategoryEndpoint.RequestModel,
    Results<Ok, BadRequest<Error>>>
{
    public override void Configure()
    {
        AllowAnonymous();
        Delete("api/workspaces/{workspaceId:guid}/expense-categories/{expenseCategoryId:guid}");
        Options(x =>
        {
            x.WithName(nameof(DeleteExpenseCategoryEndpoint));
            x.WithTags(EndpointTags.ExpenseCategories);
        });
    }

    public override async Task<Results<Ok, BadRequest<Error>>> ExecuteAsync(RequestModel req, CancellationToken ct)
    {
        var query = new DeleteExpenseCategoryCommand(req.ExpenseCategoryId, req.WorkspaceId);

        var result = await sender.Send(query, ct);

        return result.ToDefaultApiResponse();
    }

    internal sealed record RequestModel(Guid WorkspaceId, Guid ExpenseCategoryId);
}