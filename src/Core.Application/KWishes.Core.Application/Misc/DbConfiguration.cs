using System.Diagnostics.CodeAnalysis;

namespace KWishes.Core.Application.Misc;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public class DbConfiguration
{
    public string Host { get; init; } = null!;
    public int Port { get; init; } = 5432;
    public string Username { get; init; } = null!;
    public string Password { get; init; } = null!;

    public int ContextPoolSize { get; init; } = 1024;
    
    public string GetConnectionString()
    {
        return $"Host={Host};Port={Port};Username={Username};Password={Password};Database=kwishes";
    }
    
    public string GetConnectionStringWithoutPassword()
    {
        return $"Host={Host}; Port={Port}; Username={Username}; Database=kwishes";
    }
}