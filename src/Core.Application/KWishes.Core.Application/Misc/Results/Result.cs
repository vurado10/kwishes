namespace KWishes.Core.Application.Misc.Results;

public static class Result
{
    public static Result<Nothing> Ok() => Ok(new Nothing());

    public static Result<TValue> Ok<TValue>(TValue value) => new(value, default!, true);

    public static Result<TValue> Error<TValue>(ErrorInfo errorInfo) => new(default!, errorInfo, false);
    
    public static Result<TValue> Error<TValue>(
        string message, 
        ErrorInfoCode code = ErrorInfoCode.Any, 
        IReadOnlyDictionary<string, object>? details = null)
    {
        return new Result<TValue>(default!, new ErrorInfo(message, code, details), false);
    }
}