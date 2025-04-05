using System.Net;
using CabaVS.ExpenseTracker.Application.Features.Categories.Models;
using CabaVS.ExpenseTracker.Application.Features.Categories.Queries;
using CabaVS.ExpenseTracker.Application.Features.Currencies.Models;
using CabaVS.ExpenseTracker.Domain.Enums;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Presentation.Extensions;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CabaVS.ExpenseTracker.Presentation.Endpoints.Categories;

internal sealed class GetCategoryByIdEndpoint(ISender sender) : Endpoint<
    GetCategoryByIdEndpoint.RequestModel,
    Results<Ok<GetCategoryByIdEndpoint.ResponseModel>, BadRequest<Error>>>
{
    public override void Configure()
    {
        Get("/api/workspaces/{workspaceId:guid}/categories/{categoryId:guid}");
        Options(x =>
        {
            x.WithName(nameof(GetCategoryByIdEndpoint));
            x.WithTags(EndpointTags.Categories);
        });
    }

    public override async Task<Results<Ok<ResponseModel>, BadRequest<Error>>> ExecuteAsync(
        RequestModel req, CancellationToken ct)
    {
        var query = new GetCategoryByIdQuery(req.WorkspaceId, req.CategoryId);
        
        Result<CategoryModel> result = await sender.Send(query, ct);

        return result.ToDefaultApiResponse(category => new ResponseModel(category));
    }

    internal sealed record RequestModel(Guid WorkspaceId, Guid CategoryId);
    internal sealed record ResponseModel(CategoryModel CategoryDetails);
    
    internal sealed class EndpointSummary : Summary<GetCategoryByIdEndpoint>
    {
        public EndpointSummary()
        {
            Summary = "Get a Category by ID";
            Description = "Get a Category by ID within specified workspace.";
        
            Response(
                (int)HttpStatusCode.OK,
                "OK response with body.",
                example: new ResponseModel(
                    new CategoryModel(
                        Guid.NewGuid(),
                        "My USD card",
                        CategoryType.Expense,
                        new CurrencySlimModel(
                            Guid.NewGuid(),
                            "USD"))));
        
            Response(
                (int)HttpStatusCode.BadRequest,
                "Bad Request with Error.",
                example: new Error(
                    "Validation.Unspecified",
                    "Unspecified validation error occured. Check your input and try again."));
        }
    }
}
