namespace KWishes.Core.Application.Misc.Results;

public record ErrorInfo(
    string Message,
    ErrorInfoCode Code = ErrorInfoCode.Any,
    IReadOnlyDictionary<string, object>? Details = null
);