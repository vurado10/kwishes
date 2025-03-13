namespace KWishes.Core.Application.Misc;

// ReSharper disable once ClassNeverInstantiated.Global
public class AuthenticationConfiguration
{
    public bool IsAutoRedirectEnabled { get; init; } = false;
    public string ClientId { get; init; } = null!;
    public string ClientSecret { get; init; } = null!;
}