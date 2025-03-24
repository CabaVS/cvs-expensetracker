using System.Net;
using CabaVS.ExpenseTracker.Application.Contracts.Presentation;
using CabaVS.ExpenseTracker.Application.Features.Users.Models;
using CabaVS.ExpenseTracker.Application.Features.Users.Queries;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Presentation.Extensions;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CabaVS.ExpenseTracker.Presentation.Endpoints.Users;

internal sealed class GetCurrentUserEndpoint(ISender sender, ICurrentUserAccessor currentUserAccessor)
    : EndpointWithoutRequest<
        Results<Ok<GetCurrentUserEndpoint.ResponseModel>, BadRequest<Error>>>
{
    public override void Configure()
    {
        AllowAnonymous();
        Get("/api/users/me");
        Options(x =>
        {
            x.WithName(nameof(GetCurrentUserEndpoint));
            x.WithTags(EndpointTags.Users);
        });
    }

    public override async Task<Results<Ok<ResponseModel>, BadRequest<Error>>> ExecuteAsync(CancellationToken ct)
    {
        var query = new GetUserByIdQuery(currentUserAccessor.UserId);
        
        Result<UserModel> result = await sender.Send(query, ct);

        return result.ToDefaultApiResponse(userModel => new ResponseModel(userModel));
    }
    
    internal sealed record ResponseModel(UserModel User);
    
    internal sealed class GetCurrentUserEndpointSummary : Summary<GetCurrentUserEndpoint>
    {
        public GetCurrentUserEndpointSummary()
        {
            Summary = "Get Current User";
            Description = "Gets a Current User details.";
        
            Response(
                (int)HttpStatusCode.OK,
                "OK response with body.",
                example: new ResponseModel(
                    new UserModel(
                        Guid.NewGuid(),
                        "Alan Wake",
                        true)));
        
            Response(
                (int)HttpStatusCode.BadRequest,
                "Bad Request with Error.",
                example: new Error(
                    "Validation.Unspecified",
                    "Unspecified validation error occured. Check your input and try again."));
        }
    }
}
