namespace CabaVS.ExpenseTracker.Application.Common;

public interface IUserOwningRequest
{
    Guid UserId { get; }
}
