using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Primitives;
using CabaVS.ExpenseTracker.Domain.Shared;

namespace CabaVS.ExpenseTracker.Domain.ValueObjects;

public sealed class UserName : ValueObject
{
    public const int MaxLength = 50;
    
    public string Value { get; }

    private UserName(string value) => 
        Value = value;

    protected override IEnumerable<object> AtomicValues
    {
        get
        {
            yield return Value;
        }
    }

    public static Result<UserName> Create(string userName) =>
        Result<string>.Success(userName)
            .EnsureStringNotNullOrWhitespace(UserErrors.UserNameIsNullOrWhitespace())
            .EnsureStringNotTooLong(MaxLength, UserErrors.UserNameIsTooLong(userName))
            .Map(x => new UserName(x));
}
