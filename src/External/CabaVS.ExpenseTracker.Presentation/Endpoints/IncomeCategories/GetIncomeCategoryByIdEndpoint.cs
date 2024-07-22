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

namespace CabaVS.ExpenseTracker.Presentation.Endpoints.IncomeCategories;

internal sealed class GetIncomeCategoryByIdEndpoint(ISender sender) 
    : Endpoint<
        GetIncomeCategoryByIdEndpoint.RequestModel,
        Results<Ok<IncomeCategoryModel>, BadRequest<Error>>>
{
    public override void Configure()
    {
        Get("api/workspaces/{workspaceId:guid}/income-categories/{incomeCategoryId:guid}");
        Options(x =>
        {
            x.WithName(nameof(GetIncomeCategoryByIdEndpoint));
            x.WithTags(EndpointTags.IncomeCategories);
        });
    }

    public override async Task<Results<Ok<IncomeCategoryModel>, BadRequest<Error>>> ExecuteAsync(
        RequestModel req,
        CancellationToken ct)
    {
        var query = new GetIncomeCategoryByIdQuery(req.WorkspaceId, req.IncomeCategoryId);

        var result = await sender.Send(query, ct);

        return result.ToDefaultApiResponse();
    }

    internal sealed record RequestModel(Guid WorkspaceId, Guid IncomeCategoryId);
}

internal sealed class GetIncomeCategoryByIdEndpointSummary : Summary<GetIncomeCategoryByIdEndpoint>
{
    public GetIncomeCategoryByIdEndpointSummary()
    {
        Summary = "Get Income Category by ID";
        Description = "Gets an Income Category by provided ID.";

        Params[nameof(GetIncomeCategoryByIdEndpoint.RequestModel.WorkspaceId)] = 
            "Workspace ID (simple GUID, user should have access to it).";
        Params[nameof(GetIncomeCategoryByIdEndpoint.RequestModel.IncomeCategoryId)] = 
            "Income Category ID to search by (simple GUID).";
        
        Response(
            (int)HttpStatusCode.OK,
            "OK response with body.",
            example: new IncomeCategoryModel(
                new Guid("0D64417A-AC39-44C5-857A-01BECE08700D"),
                "My Company",
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