using System.Net;
using CabaVS.ExpenseTracker.Application.Features.Categories.Commands;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Presentation.Extensions;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CabaVS.ExpenseTracker.Presentation.Endpoints.IncomeCategories;

internal sealed class CreateIncomeCategoryEndpoint(ISender sender) : Endpoint<
    CreateIncomeCategoryEndpoint.RequestModel,
    Results<CreatedAtRoute, BadRequest<Error>>>
{
    public override void Configure()
    {
        AllowAnonymous();
        Post("api/workspaces/{workspaceId:guid}/income-categories");
        Options(x =>
        {
            x.WithName(nameof(CreateIncomeCategoryEndpoint));
            x.WithTags(EndpointTags.IncomeCategories);
        });
    }

    public override async Task<Results<CreatedAtRoute, BadRequest<Error>>> ExecuteAsync(RequestModel req, CancellationToken ct)
    {
        var command = new CreateIncomeCategoryCommand(
            req.Name,
            req.CurrencyId,
            req.WorkspaceId);

        var result = await sender.Send(command, ct);
        
        return result.ToDefaultApiResponse(
            nameof(GetIncomeCategoryByIdEndpoint),
            id => new { req.WorkspaceId, BalanceId = id });
    }

    public sealed record RequestModel(string Name, Guid CurrencyId, Guid WorkspaceId);
}

internal sealed class CreateIncomeCategoryEndpointSummary : Summary<CreateIncomeCategoryEndpoint>
{
    public CreateIncomeCategoryEndpointSummary()
    {
        Summary = "Create an Income Category.";
        Description = "Creates a new Income Category.";
        
        Params[nameof(CreateIncomeCategoryEndpoint.RequestModel.WorkspaceId)] = 
            "Workspace ID (simple GUID, user should have access to it).";

        ExampleRequest =
            new CreateIncomeCategoryEndpoint.RequestModel(
                "My Company",
                new Guid("8B98FA4E-BACA-4D78-85EE-802B16E473AE"),
                Guid.Empty);
        
        Response(
            (int)HttpStatusCode.Created,
            "Created At response with Location header filled.");
        
        Response(
            (int)HttpStatusCode.BadRequest,
            "Bad Request with Error.",
            example: new Error(
                "Validation.Unspecified",
                "Unspecified validation error occured. Check your input and try again."));
    }
}