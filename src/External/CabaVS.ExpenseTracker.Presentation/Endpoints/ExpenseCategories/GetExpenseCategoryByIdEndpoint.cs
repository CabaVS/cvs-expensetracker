using System.Net;
using CabaVS.ExpenseTracker.Application.Features.Categories.Models;
using CabaVS.ExpenseTracker.Application.Features.Categories.Queries;
using CabaVS.ExpenseTracker.Application.Features.Currencies.Models;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Presentation.Extensions;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CabaVS.ExpenseTracker.Presentation.Endpoints.ExpenseCategories;

internal sealed class GetExpenseCategoryByIdEndpoint(ISender sender) 
    : Endpoint<
        GetExpenseCategoryByIdEndpoint.RequestModel,
        Results<Ok<ExpenseCategoryModel>, BadRequest<Error>>>
{
    public override void Configure()
    {
        AllowAnonymous();
        Get("api/workspaces/{workspaceId:guid}/expense-categories/{expenseCategoryId:guid}");
        Options(x =>
        {
            x.WithName(nameof(GetExpenseCategoryByIdEndpoint));
            x.WithTags(EndpointTags.ExpenseCategories);
        });
    }

    public override async Task<Results<Ok<ExpenseCategoryModel>, BadRequest<Error>>> ExecuteAsync(
        RequestModel req,
        CancellationToken ct)
    {
        var query = new GetExpenseCategoryByIdQuery(req.WorkspaceId, req.ExpenseCategoryId);

        var result = await sender.Send(query, ct);

        return result.ToDefaultApiResponse();
    }

    internal sealed record RequestModel(Guid WorkspaceId, Guid ExpenseCategoryId);
}

internal sealed class GetExpenseCategoryByIdEndpointSummary : Summary<GetExpenseCategoryByIdEndpoint>
{
    public GetExpenseCategoryByIdEndpointSummary()
    {
        Summary = "Get Expense Category by ID";
        Description = "Gets an Expense Category by provided ID.";

        Params[nameof(GetExpenseCategoryByIdEndpoint.RequestModel.WorkspaceId)] = 
            "Workspace ID (simple GUID, user should have access to it).";
        Params[nameof(GetExpenseCategoryByIdEndpoint.RequestModel.ExpenseCategoryId)] = 
            "Expense Category ID to search by (simple GUID).";
        
        Response(
            (int)HttpStatusCode.OK,
            "OK response with body.",
            example: new ExpenseCategoryModel(
                new Guid("0D64417A-AC39-44C5-857A-01BECE08700D"),
                "Household",
                new CurrencyModel(
                    new Guid("AE6320FD-AE15-44AE-B65D-18C6541042DD"),
                    "United States Dollar",
                    "USD",
                    "$")));
        
        Response(
            (int)HttpStatusCode.BadRequest,
            "Bad Request with Error.",
            example: new Error(
                "Validation.Unspecified",
                "Unspecified validation error occured. Check your input and try again."));
    }
}