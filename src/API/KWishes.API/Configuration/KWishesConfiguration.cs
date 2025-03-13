using KWishes.Core.Application.Misc;

namespace KWishes.API.Configuration;

// ReSharper disable once ClassNeverInstantiated.Global
public class KWishesConfiguration
{
    public DbConfiguration Db { get; init; } = null!;
    public StaticFilesConfiguration Static { get; init; } = null!;
    public AuthenticationConfiguration Authentication { get; init; } = null!;
}