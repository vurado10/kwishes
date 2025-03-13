using KWishes.Core.Domain;
using KWishes.Core.Domain.Comments;
using KWishes.Core.Domain.Products;
using KWishes.Core.Domain.Users;
using KWishes.Core.Domain.Votes;
using KWishes.Core.Domain.Wishes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging.Abstractions;
using Npgsql;

namespace KWishes.Core.Application.Misc;

public sealed class RepositoryContext : DbContext
{
    public DbSet<User> Users { get; }
    public DbSet<Product> Products { get; }
    public DbSet<Wish> Wishes { get; }
    public DbSet<Comment> Comments { get; }
    public DbSet<Vote> Votes { get; }

    public RepositoryContext(DbContextOptions<RepositoryContext> options) 
        : base(options)
    {
        Users = Set<User>();
        Products = Set<Product>();
        Wishes = Set<Wish>();
        Comments = Set<Comment>();
        Votes = Set<Vote>();
    }

    public static void SetOptions(DbContextOptionsBuilder optionsBuilder, DbConfiguration configuration)
    {
        optionsBuilder
            .UseNpgsql(GetNpgsqlDataSource(configuration.GetConnectionString()))
            .UseSnakeCaseNamingConvention()
            .UseLoggerFactory(NullLoggerFactory.Instance);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RepositoryContext).Assembly);

        modelBuilder
            .HasPostgresEnum<Role>()
            .HasPostgresEnum<WishStatus>()
            .HasPostgresEnum<VoteType>();
    }

    private static NpgsqlDataSource GetNpgsqlDataSource(string? connectionString = null)
    {
        connectionString ??= new DbConfiguration { Host = "host" }.GetConnectionString();
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);

        dataSourceBuilder
            .MapEnum<Role>()
            .MapEnum<WishStatus>()
            .MapEnum<VoteType>();

        return dataSourceBuilder.Build();
    }
    
    // ReSharper disable once UnusedType.Local
    private sealed class Factory : IDesignTimeDbContextFactory<RepositoryContext>
    {
        public RepositoryContext CreateDbContext(string[] args) =>
            new(new DbContextOptionsBuilder<RepositoryContext>()
                .UseNpgsql(GetNpgsqlDataSource())
                .UseSnakeCaseNamingConvention()
                .Options);
    }
}