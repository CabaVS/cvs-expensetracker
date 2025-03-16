namespace CabaVS.ExpenseTracker.Domain.Shared;

public static class ResultExtensions
{
    public static Result<T> Ensure<T>(this Result<T> result, Func<T, bool> condition, Error error)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(condition);
        
        return result.IsFailure
            ? result
            : condition(result.Value)
                ? result
                : Result<T>.Failure(error);
    }
    
    public static Result<TOut> Map<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> mapFunc)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(mapFunc);
        
        return result.IsSuccess
            ? Result<TOut>.Success(mapFunc(result.Value))
            : Result<TOut>.Failure(result.Error);
    }

    public static Result<TOut> Map<TIn, TOut>(this Result<TIn> result, Func<TIn, Result<TOut>> mapFunc)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(mapFunc);
        
        return result.IsSuccess
            ? mapFunc(result.Value)
            : Result<TOut>.Failure(result.Error);
    }

    public static Result<string> EnsureStringNotNullOrWhitespace(this Result<string> result, Error error) =>
        result.Ensure(
            x => !string.IsNullOrWhiteSpace(x),
            error);

    public static Result<string> EnsureStringNotTooLong(this Result<string> result, int maxLength, Error error) =>
        result.Ensure(
            x => x.Length <= maxLength,
            error);

    public static Result<string> EnsureStringNotTooShort(this Result<string> result, int minLength, Error error) =>
        result.Ensure(
            x => x.Length >= minLength,
            error);
}