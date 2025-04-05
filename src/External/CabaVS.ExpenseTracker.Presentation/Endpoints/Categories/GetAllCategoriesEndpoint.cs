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

internal sealed class GetAllCategoriesEndpoint(ISender sender) : Endpoint<
    GetAllCategoriesEndpoint.RequestModel,
    Results<Ok<GetAllCategoriesEndpoint.ResponseModel>, BadRequest<Error>>>
{
    public override void Configure()
    {
        Get("/api/workspaces/{workspaceId:guid}/categories");
        Options(x =>
        {
            x.WithName(nameof(GetAllCategoriesEndpoint));
            x.WithTags(EndpointTags.Categories);
        });
    }

    public override async Task<Results<Ok<ResponseModel>, BadRequest<Error>>> ExecuteAsync(
        RequestModel req, CancellationToken ct)
    {
        var query = new GetAllCategoriesQuery(req.WorkspaceId);
        
        Result<CategoryModel[]> result = await sender.Send(query, ct);

        return result.ToDefaultApiResponse(categories => new ResponseModel(categories));
    }

    internal sealed record RequestModel(Guid WorkspaceId);
    internal sealed record ResponseModel(CategoryModel[] Categories);
    
    internal sealed class EndpointSummary : Summary<GetAllCategoriesEndpoint>
    {
        public EndpointSummary()
        {
            Summary = "Get all Categories";
            Description = "Get all Categories within specified workspace.";
        
            Response(
                (int)HttpStatusCode.OK,
                "OK response with body.",
                example: new ResponseModel(
                [
                    new CategoryModel(
                        Guid.NewGuid(),
                        "My Expense Category",
                        CategoryType.Expense,
                        new CurrencySlimModel(
                            Guid.NewGuid(),
                            "USD")),
                    new CategoryModel(
                        Guid.NewGuid(),
                        "My Income Category",
                        CategoryType.Income,
                        new CurrencySlimModel(
                            Guid.NewGuid(),
                            "PLN"))
                ]));
        
            Response(
                (int)HttpStatusCode.BadRequest,
                "Bad Request with Error.",
                example: new Error(
                    "Validation.Unspecified",
                    "Unspecified validation error occured. Check your input and try again."));
        }
    }
}
