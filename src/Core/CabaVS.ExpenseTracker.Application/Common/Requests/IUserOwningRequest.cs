namespace CabaVS.ExpenseTracker.Application.Common.Requests;

public interface IUserOwningRequest
{
    Guid UserId { get; }
}
