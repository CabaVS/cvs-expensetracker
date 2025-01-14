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
        var command = new CreateBalanceCommand(req.WorkspaceId, req.CurrencyId, req.Name, req.Amount);

        Result<Guid> result = await sender.Send(command, ct);
        
        return result.ToDefaultApiResponse(
            nameof(GetBalanceByIdEndpoint),
            id => new { BalanceId = id });
    }
    
    internal sealed record RequestModel(string Name, decimal Amount, Guid CurrencyId, Guid WorkspaceId);
}

internal sealed class CreateBalanceEndpointSummary : Summary<CreateBalanceEndpoint>
{
    public CreateBalanceEndpointSummary()
    {
        Summary = "Create a Balance.";
        Description = "Creates a new Balance.";
        
        Params[nameof(GetBalanceByIdEndpoint.GetBalanceByIdEndpointRequest.WorkspaceId)] = 
            "Workspace ID to verify access.";

        ExampleRequest =
            new CreateBalanceEndpoint.RequestModel(
                "My USD card",
                1234.56m,
                new Guid("D0591D50-FCE7-43DD-BAD6-739F831D6F38"),
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
