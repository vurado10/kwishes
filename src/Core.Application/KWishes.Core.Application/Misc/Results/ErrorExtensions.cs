namespace KWishes.Core.Application.Misc.Results;

public static class ErrorInfoExtensions
{
    public static Result<TValue> ToResult<TValue>(this ErrorInfo errorInfo) => Result.Error<TValue>(errorInfo);
    
    public static Result<Nothing> ToResult(this ErrorInfo errorInfo) => Result.Error<Nothing>(errorInfo);
}