using System.Net;
using CabaVS.ExpenseTracker.Application.Features.Categories.Commands;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Presentation.Extensions;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CabaVS.ExpenseTracker.Presentation.Endpoints.ExpenseCategories;

internal sealed class CreateExpenseCategoryEndpoint(ISender sender) : Endpoint<
    CreateExpenseCategoryEndpoint.RequestModel,
    Results<CreatedAtRoute, BadRequest<Error>>>
{
    public override void Configure()
    {
        AllowAnonymous();
        Post("api/workspaces/{workspaceId:guid}/expense-categories");
        Options(x =>
        {
            x.WithName(nameof(CreateExpenseCategoryEndpoint));
            x.WithTags(EndpointTags.ExpenseCategories);
        });
    }

    public override async Task<Results<CreatedAtRoute, BadRequest<Error>>> ExecuteAsync(RequestModel req, CancellationToken ct)
    {
        var command = new CreateExpenseCategoryCommand(
            req.Name,
            req.CurrencyId,
            req.WorkspaceId);

        var result = await sender.Send(command, ct);
        
        return result.ToDefaultApiResponse(
            nameof(CreateExpenseCategoryEndpoint), // TODO
            id => new { req.WorkspaceId, ExpenseCategoryId = id });
    }

    public sealed record RequestModel(string Name, Guid CurrencyId, Guid WorkspaceId);
}

internal sealed class CreateExpenseCategoryEndpointSummary : Summary<CreateExpenseCategoryEndpoint>
{
    public CreateExpenseCategoryEndpointSummary()
    {
        Summary = "Create an Expense Category.";
        Description = "Creates a new Expense Category.";
        
        Params[nameof(CreateExpenseCategoryEndpoint.RequestModel.WorkspaceId)] = 
            "Workspace ID (simple GUID, user should have access to it).";

        ExampleRequest =
            new CreateExpenseCategoryEndpoint.RequestModel(
                "Household",
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