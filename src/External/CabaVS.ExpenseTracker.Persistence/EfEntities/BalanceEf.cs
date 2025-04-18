﻿using System.Linq.Expressions;
using CabaVS.ExpenseTracker.Domain.Entities;

namespace CabaVS.ExpenseTracker.Persistence.EfEntities;

internal sealed class BalanceEf
{
    public Guid Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime ModifiedOn { get; set; }

    public string Name { get; set; } = string.Empty;
    public decimal Amount { get; set; }

    public Guid WorkspaceId { get; set; }
    public WorkspaceEf? Workspace { get; set; }

    public Guid CurrencyId { get; set; }
    public CurrencyEf? Currency { get; set; }
    
    internal static BalanceEf FromDomain(Balance balance, Guid workspaceId) => new()
    {
        Id = balance.Id,
        CreatedOn = balance.CreatedOn,
        ModifiedOn = balance.ModifiedOn,
        Name = balance.Name.Value,
        Amount = balance.Amount,
        WorkspaceId = workspaceId,
        CurrencyId = balance.Currency.Id
    };

    internal static Expression<Func<BalanceEf, Balance>> ProjectToDomain =>
        b => Balance
            .CreateExisting(
                b.Id,
                b.CreatedOn,
                b.ModifiedOn,
                b.Name,
                b.Amount,
                Domain.Entities.Currency
                    .CreateExisting(
                        b.Currency!.Id,
                        b.Currency.CreatedOn,
                        b.Currency.ModifiedOn,
                        b.Currency.Name,
                        b.Currency.Code,
                        b.Currency.Symbol)
                    .Value)
            .Value;
}
