using System.Net;
using CabaVS.ExpenseTracker.Application.Features.Reports.Models;
using CabaVS.ExpenseTracker.Application.Features.Reports.Queries;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Presentation.Extensions;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CabaVS.ExpenseTracker.Presentation.Endpoints.Reports;

internal sealed class GetIncomeExpenseReportEndpoint(ISender sender) 
    : Endpoint<
        GetIncomeExpenseReportEndpoint.RequestModel,
        Results<Ok<IncomeExpenseReportModel>, BadRequest<Error>>>
{
    public override void Configure()
    {
        Get("api/workspaces/{workspaceId:guid}/reports/income-expense");
        Options(x =>
        {
            x.WithName(nameof(GetIncomeExpenseReportEndpoint));
            x.WithTags(EndpointTags.Reports);
        });
    }

    public override async Task<Results<Ok<IncomeExpenseReportModel>, BadRequest<Error>>> ExecuteAsync(
        RequestModel req, CancellationToken ct)
    {
        var query = new GetIncomeExpenseReportByDateQuery(req.WorkspaceId, req.From, req.To);

        Result<IncomeExpenseReportModel> result = await sender.Send(query, ct);

        return result.ToDefaultApiResponse();
    }
    
    internal sealed record RequestModel(Guid WorkspaceId, DateOnly From, DateOnly To);
}

internal sealed class GetIncomeExpenseReportEndpointSummary : Summary<GetIncomeExpenseReportEndpoint>
{
    public GetIncomeExpenseReportEndpointSummary()
    {
        Summary = "Get Income/Expense Report";
        Description = "Gets an Income/Expense report over the provided workspace grouped by currency code.";
        
        Params[nameof(GetIncomeExpenseReportEndpoint.RequestModel.WorkspaceId)] = 
            "Workspace ID to verify access.";

        ExampleRequest =
            new GetIncomeExpenseReportEndpoint.RequestModel(
                Guid.Empty,
                new DateOnly(2020, 1, 1),
                new DateOnly(2020, 1, 31));
        
        Response(
            (int)HttpStatusCode.OK,
            "OK response with body.",
            example: new IncomeExpenseReportModel(
                [
                    new IncomeExpenseReportModel.ReportLine(1234.56m, "USD"),
                    new IncomeExpenseReportModel.ReportLine(12.34m, "PLN")
                ],
                [
                    new IncomeExpenseReportModel.ReportLine(-789.0m, "USD"),
                    new IncomeExpenseReportModel.ReportLine(-1000.00m, "EUR")
                ],
                [
                    new IncomeExpenseReportModel.ReportLine(445.56m, "USD"),
                    new IncomeExpenseReportModel.ReportLine(12.34m, "PLN"),
                    new IncomeExpenseReportModel.ReportLine(-1000.00m, "EUR"),
                ]));
        
        Response(
            (int)HttpStatusCode.BadRequest,
            "Bad Request with Error.",
            example: new Error(
                "Validation.Unspecified",
                "Unspecified validation error occured. Check your input and try again."));
    }
}
