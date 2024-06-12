using System.Net;
using CabaVS.ExpenseTracker.Application.Features.Balances.Commands;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Presentation.Extensions;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CabaVS.ExpenseTracker.Presentation.Endpoints.Balances;

internal sealed class CreateBalanceEndpoint(ISender sender) 
    : Endpoint<
        CreateBalanceEndpoint.RequestModel,
        Results<CreatedAtRoute, BadRequest<Error>>>
{
    public override void Configure()
    {
        AllowAnonymous();
        Post("api/workspaces/{workspaceId:guid}/balances");
        Options(x =>
        {
            x.WithName(nameof(CreateBalanceEndpoint));
            x.WithTags(EndpointTags.Balances);
        });
    }

    public override async Task<Results<CreatedAtRoute, BadRequest<Error>>> ExecuteAsync(
        RequestModel req,
        CancellationToken ct)
    {
        var command = new CreateBalanceCommand(
            req.Name,
            req.Amount,
            req.CurrencyId,
            req.WorkspaceId);

        var result = await sender.Send(command, ct);
        
        return result.ToDefaultApiResponse(nameof(CreateBalanceEndpoint)); // TODO: Wrong endpoint
    }
    
    internal sealed record RequestModel(Guid WorkspaceId, string Name, decimal Amount, Guid CurrencyId);
}

internal sealed class CreateBalanceEndpointSummary : Summary<CreateBalanceEndpoint>
{
    public CreateBalanceEndpointSummary()
    {
        Summary = "Create a Balance.";
        Description = "Creates a new Balance.";
        
        Params[nameof(CreateBalanceEndpoint.RequestModel.WorkspaceId)] = 
            "Workspace ID (simple GUID, user should have access to it).";

        ExampleRequest =
            new CreateBalanceEndpoint.RequestModel(
                default,
                "Visa EU",
                1000,
                new Guid("8B98FA4E-BACA-4D78-85EE-802B16E473AE"));
        
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