namespace CabaVS.ExpenseTracker.Domain.Shared;

public class Result
{
    protected Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None ||
            !isSuccess && error == Error.None)
        {
            throw new ArgumentException("Invalid error in Result constructor.", nameof(error));
        }
        
        IsSuccess = isSuccess;
        _error = error;
    }

    private readonly Error _error;
    
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    public Error Error => IsFailure
        ? _error
        : throw new InvalidOperationException($"Unable to access '{nameof(Error)}' property on successful Result.");

    public static Result Success() => new(true, Error.None);
    public static Result Failure(Error error) => new(false, error);
    
    public static implicit operator Result(Error error) => Failure(error);
}

public sealed class Result<T> : Result
{
    private Result(bool isSuccess, Error error, T? value) : base(isSuccess, error)
    {
        _value = value;
    }

    private readonly T? _value;
    
    public T Value =>
        IsSuccess
            ? _value!
            : throw new InvalidOperationException($"Unable to access '{nameof(Value)}' property on failed Result.");
    
    public static Result<T> Success(T value) => new(true, Error.None, value);
    public new static Result<T> Failure(Error error) => new(false, error, default);
    
    public static implicit operator Result<T>(T value) => Success(value);
    public static implicit operator Result<T>(Error error) => Failure(error);
}