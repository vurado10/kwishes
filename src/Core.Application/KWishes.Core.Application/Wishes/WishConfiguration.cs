using KWishes.Core.Domain.Wishes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KWishes.Core.Application.Wishes;

public class WishConfiguration : IEntityTypeConfiguration<Wish>
{
    public void Configure(EntityTypeBuilder<Wish> builder)
    {
        builder.ToTable("wishes");
        
        builder
            .Property(u => u.Id)
            .HasConversion(id => id.Value, guid => new WishId(guid))
            .ValueGeneratedOnAdd();

        builder
            .HasOne(y => y.Product)
            .WithMany(x => x.Wishes)
            .HasForeignKey(e => e.ProductId)
            .IsRequired();
        
        builder
            .HasOne(y => y.Creator)
            .WithMany()
            .HasForeignKey(e => e.CreatorId)
            .IsRequired();
        
        builder
            .HasIndex(w => w.ProductId)
            .HasDatabaseName("ix_product_id_1");
        
        builder
            .HasIndex(w => w.CreatorId)
            .HasDatabaseName("ix_creator_id_1");
    }
}