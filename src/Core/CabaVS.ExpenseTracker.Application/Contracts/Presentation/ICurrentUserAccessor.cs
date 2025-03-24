namespace CabaVS.ExpenseTracker.Application.Contracts.Presentation;

public interface ICurrentUserAccessor
{
    Guid UserId { get; }
}
