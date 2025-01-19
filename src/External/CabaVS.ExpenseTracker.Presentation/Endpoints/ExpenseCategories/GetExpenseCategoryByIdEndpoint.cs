using System.Net;
using CabaVS.ExpenseTracker.Application.Features.Currencies.Models;
using CabaVS.ExpenseTracker.Application.Features.ExpenseCategories.Models;
using CabaVS.ExpenseTracker.Application.Features.ExpenseCategories.Queries;
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
        GetExpenseCategoryByIdEndpoint.GetExpenseCategoryByIdEndpointRequest,
        Results<Ok<ExpenseCategoryModel>, BadRequest<Error>>>
{
    public override void Configure()
    {
        Get("api/workspaces/{workspaceId:guid}/expense-categories/{expenseCategoryId:guid}");
        Options(x =>
        {
            x.WithName(nameof(GetExpenseCategoryByIdEndpoint));
            x.WithTags(EndpointTags.ExpenseCategories);
        });
    }

    public override async Task<Results<Ok<ExpenseCategoryModel>, BadRequest<Error>>> ExecuteAsync(
        GetExpenseCategoryByIdEndpointRequest req,
        CancellationToken ct)
    {
        var query = new GetExpenseCategoryByIdQuery(req.WorkspaceId, req.ExpenseCategoryId);

        Result<ExpenseCategoryModel> result = await sender.Send(query, ct);

        return result.ToDefaultApiResponse();
    }

    internal sealed record GetExpenseCategoryByIdEndpointRequest(Guid WorkspaceId, Guid ExpenseCategoryId);
}

internal sealed class GetExpenseCategoryByIdEndpointSummary : Summary<GetExpenseCategoryByIdEndpoint>
{
    public GetExpenseCategoryByIdEndpointSummary()
    {
        Summary = "Get Expense Category by ID";
        Description = "Gets an Expense Category by provided ID. Ensures that User is a member of the parent Workspace.";

        Params[nameof(GetExpenseCategoryByIdEndpoint.GetExpenseCategoryByIdEndpointRequest.WorkspaceId)] = 
            "Workspace ID to verify access.";
        Params[nameof(GetExpenseCategoryByIdEndpoint.GetExpenseCategoryByIdEndpointRequest.ExpenseCategoryId)] = 
            "Expense Category ID to search by (simple GUID).";
        
        Response(
            (int)HttpStatusCode.OK,
            "OK response with body.",
            example: new ExpenseCategoryModel(
                new Guid("DCF2732A-E43A-4CB1-8718-EA1AD3543BA1"),
                "Transportation",
                new CurrencyModel(
                    new Guid("9BC67337-3BB5-469E-A71C-B995305F379B"),
                    "United States dollar",
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
