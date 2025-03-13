using System.Diagnostics.CodeAnalysis;

namespace KWishes.Core.Application.Misc.Results;

public readonly record struct Result<TValue>(TValue? Value, ErrorInfo? Error, bool IsOk)
{
    public bool TryGetError([NotNullWhen(false)] out TValue? value, [NotNullWhen(true)] out ErrorInfo? error)
    {
        value = IsOk ? Value : default;
        error = !IsOk ? Error : null;

        return !IsOk;
    }
}