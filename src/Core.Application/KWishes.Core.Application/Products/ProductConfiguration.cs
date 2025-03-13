using KWishes.Core.Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KWishes.Core.Application.Products;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products");
        
        builder
            .Property(p => p.Logo)
            .HasConversion(
                uri => uri.AbsoluteUri,
                s => new Uri(s));
        
        builder
            .HasIndex(product => product.Name)
            .HasDatabaseName("ix_name_1")
            .IsUnique();
    }
}