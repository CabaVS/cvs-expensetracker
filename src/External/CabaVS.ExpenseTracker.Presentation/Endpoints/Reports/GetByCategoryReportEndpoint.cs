using System.Net;
using CabaVS.ExpenseTracker.Application.Features.Reports.Queries;
using CabaVS.ExpenseTracker.Application.Features.Transactions.Models;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Presentation.Extensions;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CabaVS.ExpenseTracker.Presentation.Endpoints.Reports;

internal sealed class GetByCategoryReportEndpoint(ISender sender) 
    : Endpoint<
        GetByCategoryReportEndpoint.RequestModel,
        Results<Ok<TransactionMoneyByCategoryModel[]>, BadRequest<Error>>>
{
    public override void Configure()
    {
        Get("api/workspaces/{workspaceId:guid}/reports/by-category");
        Options(x =>
        {
            x.WithName(nameof(GetByCategoryReportEndpoint));
            x.WithTags(EndpointTags.Reports);
        });
    }

    public override async Task<Results<Ok<TransactionMoneyByCategoryModel[]>, BadRequest<Error>>> ExecuteAsync(
        RequestModel req, CancellationToken ct)
    {
        var query = new GetIncomeExpenseReportByCategoryQuery(req.WorkspaceId, req.From, req.To);

        Result<TransactionMoneyByCategoryModel[]> result = await sender.Send(query, ct);

        return result.ToDefaultApiResponse();
    }
    
    internal sealed record RequestModel(Guid WorkspaceId, DateOnly From, DateOnly To);
}

internal sealed class GetByCategoryReportEndpointSummary : Summary<GetByCategoryReportEndpoint>
{
    public GetByCategoryReportEndpointSummary()
    {
        Summary = "Get By Category Report";
        Description = "Gets a By Category report over the provided workspace grouped by currency code.";
        
        Params[nameof(GetByCategoryReportEndpoint.RequestModel.WorkspaceId)] = 
            "Workspace ID to verify access.";

        ExampleRequest =
            new GetByCategoryReportEndpoint.RequestModel(
                Guid.Empty,
                new DateOnly(2020, 1, 1),
                new DateOnly(2020, 1, 31));
        
        Response(
            (int)HttpStatusCode.OK,
            "OK response with body.",
            example: new { Message = "Not Implemented." });
        
        Response(
            (int)HttpStatusCode.BadRequest,
            "Bad Request with Error.",
            example: new Error(
                "Validation.Unspecified",
                "Unspecified validation error occured. Check your input and try again."));
    }
}
