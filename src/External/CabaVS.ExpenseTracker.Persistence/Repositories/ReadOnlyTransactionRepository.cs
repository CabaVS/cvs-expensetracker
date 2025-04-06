using System.Diagnostics;
using CabaVS.ExpenseTracker.Application.Contracts.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Features.Transactions.Models;
using CabaVS.ExpenseTracker.Domain.Enums;
using CabaVS.ExpenseTracker.Persistence.Converters;
using CabaVS.ExpenseTracker.Persistence.Dapper;
using Dapper;
using Microsoft.Data.SqlClient;

namespace CabaVS.ExpenseTracker.Persistence.Repositories;

internal sealed class ReadOnlyTransactionRepository(ISqlConnectionFactory connectionFactory) : IReadOnlyTransactionRepository
{
    public async Task<TransactionModel[]> GetMultipleAsync(
        Guid workspaceId, DateOnly from, DateOnly to,
        CancellationToken cancellationToken = default)
    {
        await using SqlConnection connection = connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        
        const string sql =
            $"""
            {TransactionModelSelectAndJoinsSqlPart}
            WHERE t.[WorkspaceId] = @WorkspaceId
            AND t.[Date] >= @From
            AND t.[Date] <= @To;
            """;

        var dateOnlyConverter = new DateOnlyToDateTimeConverter();
        DateTime fromConverted = dateOnlyConverter.ConvertToProviderTyped(from);
        DateTime toConverted = dateOnlyConverter.ConvertToProviderTyped(to);
        
        IEnumerable<TransactionModel> transactionModels = await connection.QueryAsync(
            sql,
            MappingFunc,
            new { WorkspaceId = workspaceId, From = fromConverted, To = toConverted },
            splitOn: "SourceBalanceId,DestinationBalanceId");
        return [.. transactionModels];
    }
    
    public async Task<TransactionModel?> GetSingleByIdAsync(
        Guid workspaceId, Guid transactionId,
        CancellationToken cancellationToken = default)
    {
        await using SqlConnection connection = connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        
        const string sql =
            $"""
            {TransactionModelSelectAndJoinsSqlPart}
            WHERE t.[Id] = @TransactionId
            AND t.[WorkspaceId] = @WorkspaceId
            """;
        
        IEnumerable<TransactionModel> transactionModels = await connection.QueryAsync(
            sql,
            MappingFunc,
            new { TransactionId = transactionId, WorkspaceId = workspaceId },
            splitOn: "SourceBalanceId,DestinationBalanceId");
        return transactionModels.FirstOrDefault();
    }

    private const string TransactionModelSelectAndJoinsSqlPart =
        """
        SELECT
            t.[Id], t.[Date], t.[Type], t.[Tags],
            t.[AmountInSourceCurrency], t.[AmountInDestinationCurrency],
            -- Source joins
            sb.[Id] AS SourceBalanceId, sb.[Name] AS SourceBalanceName,
            sc.[Id] AS SourceCategoryId, sc.[Name] AS SourceCategoryName,
            -- Destination joins
            db.[Id] AS DestinationBalanceId, db.[Name] AS DestinationBalanceName,
            dc.[Id] AS DestinationCategoryId, dc.[Name] AS DestinationCategoryName
        FROM [dbo].[Transactions] t
        LEFT JOIN [dbo].[Balances] sb ON t.[SourceBalanceId] = sb.[Id]
        LEFT JOIN [dbo].[Categories] sc ON t.[SourceCategoryId] = sc.[Id]
        LEFT JOIN [dbo].[Balances] db ON t.[DestinationBalanceId] = db.[Id]
        LEFT JOIN [dbo].[Categories] dc ON t.[DestinationCategoryId] = dc.[Id]
        """;

    private static readonly
        Func<
            TransactionPartDapperModel,
            TransactionSourcePartDapperModel,
            TransactionDestinationPartDapperModel,
            TransactionModel> MappingFunc =
            (t, src, dst) => new TransactionModel(
                t.Id, new DateOnlyToDateTimeConverter().ConvertFromProviderTyped.Invoke(t.Date),
                t.Type, new StringArrayToCommaSeparatedStringConverter().ConvertFromProviderTyped.Invoke(t.Tags),
                t.AmountInSourceCurrency, t.AmountInDestinationCurrency,
                t.Type switch
                {
                    TransactionType.Expense or TransactionType.Transfer => new TransactionSideModel(
                        src.SourceBalanceId!.Value, src.SourceBalanceName!),
                    TransactionType.Income => new TransactionSideModel(src.SourceCategoryId!.Value,
                        src.SourceCategoryName!),
                    _ => throw new UnreachableException()
                },
                t.Type switch
                {
                    TransactionType.Income or TransactionType.Transfer => new TransactionSideModel(
                        dst.DestinationBalanceId!.Value, dst.DestinationBalanceName!),
                    TransactionType.Expense => new TransactionSideModel(dst.DestinationCategoryId!.Value,
                        dst.DestinationCategoryName!),
                    _ => throw new UnreachableException()
                });

    private sealed record TransactionPartDapperModel(
        Guid Id,
        DateTime Date,
        TransactionType Type,
        string Tags,
        decimal AmountInSourceCurrency,
        decimal AmountInDestinationCurrency);

    private sealed record TransactionSourcePartDapperModel(
        Guid? SourceBalanceId,
        string? SourceBalanceName,
        Guid? SourceCategoryId,
        string? SourceCategoryName);
    
    private sealed record TransactionDestinationPartDapperModel(
        Guid? DestinationBalanceId,
        string? DestinationBalanceName,
        Guid? DestinationCategoryId,
        string? DestinationCategoryName);
}
