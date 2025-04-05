using System.Net;
using CabaVS.ExpenseTracker.Application.Features.Categories.Commands;
using CabaVS.ExpenseTracker.Domain.Enums;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Presentation.Extensions;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CabaVS.ExpenseTracker.Presentation.Endpoints.Categories;

internal sealed class CreateCategoryEndpoint(ISender sender)
    : Endpoint<
        CreateCategoryEndpoint.RequestModel,
        Results<CreatedAtRoute, BadRequest<Error>>>
{
    public override void Configure()
    {
        Post("/api/workspaces/{workspaceId}/categories");
        Options(x =>
        {
            x.WithName(nameof(CreateCategoryEndpoint));
            x.WithTags(EndpointTags.Categories);
        });
    }

    public override async Task<Results<CreatedAtRoute, BadRequest<Error>>> ExecuteAsync(RequestModel req, CancellationToken ct)
    {
        var command = new CreateCategoryCommand(req.WorkspaceId, req.Name, req.Type, req.CurrencyId);
        
        Result<Guid> result = await sender.Send(command, ct);

        return result.ToDefaultApiResponse(
            nameof(GetCategoryByIdEndpoint),
            id => new
            {
                req.WorkspaceId,
                CategoryId = id
            });
    }

    internal sealed record RequestModel(Guid WorkspaceId, string Name, CategoryType Type, Guid CurrencyId);
    
    internal sealed class EndpointSummary : Summary<CreateCategoryEndpoint>
    {
        public EndpointSummary()
        {
            Summary = "Create a Category";
            Description = "Creates a Category within specified workspace.";
            
            Params[nameof(GetCategoryByIdEndpoint.RequestModel.WorkspaceId)] = 
                "Workspace ID to verify access.";

            ExampleRequest =
                new RequestModel(
                    Guid.Empty,
                    "My Category",
                    CategoryType.Expense,
                    Guid.NewGuid());
        
            Response(
                (int)HttpStatusCode.Created,
                "Created At Route response without a body. Location header is filled with Id of created entity.");
        
            Response(
                (int)HttpStatusCode.BadRequest,
                "Bad Request with Error.",
                example: new Error(
                    "Validation.Unspecified",
                    "Unspecified validation error occured. Check your input and try again."));
        }
    }
}
