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
        Post("/api/workspaces/{workspaceId}/balances");
        Options(x =>
        {
            x.WithName(nameof(CreateBalanceEndpoint));
            x.WithTags(EndpointTags.Balances);
        });
    }

    public override async Task<Results<CreatedAtRoute, BadRequest<Error>>> ExecuteAsync(RequestModel req, CancellationToken ct)
    {
        var command = new CreateBalanceCommand(req.WorkspaceId, req.Name, req.Amount, req.CurrencyId);
        
        Result<Guid> result = await sender.Send(command, ct);

        return result.ToDefaultApiResponse(
            nameof(GetBalanceByIdEndpoint),
            id => new
            {
                req.WorkspaceId,
                BalanceId = id
            });
    }

    internal sealed record RequestModel(Guid WorkspaceId, string Name, decimal Amount, Guid CurrencyId);
    
    internal sealed class EndpointSummary : Summary<CreateBalanceEndpoint>
    {
        public EndpointSummary()
        {
            Summary = "Create a Balance";
            Description = "Creates a Balance within specified workspace.";
            
            Params[nameof(RequestModel.WorkspaceId)] = "Workspace ID to verify access.";

            ExampleRequest =
                new RequestModel(
                    Guid.Empty,
                    "My Balance",
                    1234.56m,
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
