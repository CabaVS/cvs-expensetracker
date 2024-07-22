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

internal sealed class GetAllExpenseCategoriesEndpoint(ISender sender) : Endpoint<
    GetAllExpenseCategoriesEndpoint.RequestModel,
    Results<Ok<ExpenseCategoryModel[]>, BadRequest<Error>>>
{
    public override void Configure()
    {
        Get("api/workspaces/{workspaceId:guid}/expense-categories");
        Options(x =>
        {
            x.WithName(nameof(GetAllExpenseCategoriesEndpoint));
            x.WithTags(EndpointTags.ExpenseCategories);
        });
    }

    public override async Task<Results<Ok<ExpenseCategoryModel[]>, BadRequest<Error>>> ExecuteAsync(RequestModel req, CancellationToken ct)
    {
        var query = new GetAllExpenseCategoriesQuery(req.WorkspaceId);

        var result = await sender.Send(query, ct);

        return result.ToDefaultApiResponse();
    }
    
    internal sealed record RequestModel(Guid WorkspaceId);
}

internal sealed class GetAllExpenseCategoriesEndpointSummary : Summary<GetAllExpenseCategoriesEndpoint>
{
    public GetAllExpenseCategoriesEndpointSummary()
    {
        Summary = "Get all Expense Categories";
        Description = "Gets all Expense Categories.";
        
        Params[nameof(GetAllExpenseCategoriesEndpoint.RequestModel.WorkspaceId)] = 
            "Workspace ID (simple GUID, user should have access to it).";
        
        Response(
            (int)HttpStatusCode.OK,
            "OK response with body.",
            example: new[]
            {
                new ExpenseCategoryModel(
                    new Guid("0D64417A-AC39-44C5-857A-01BECE08700D"),
                    "Household",
                    new CurrencyModel(
                        new Guid("AE6320FD-AE15-44AE-B65D-18C6541042DD"),
                        "United States Dollar",
                        "USD",
                        "$")),
                new ExpenseCategoryModel(
                    new Guid("FF3D352C-FD4A-442B-8ACE-D35A80520303"),
                    "Beauty",
                    new CurrencyModel(
                        new Guid("8DC3B3C7-E1C6-4192-87A6-4A67192ADB94"),
                        "Euro",
                        "EUR",
                        "€"))
            });
        
        Response(
            (int)HttpStatusCode.BadRequest,
            "Bad Request with Error.",
            example: new Error(
                "Validation.Unspecified",
                "Unspecified validation error occured. Check your input and try again."));
    }
}