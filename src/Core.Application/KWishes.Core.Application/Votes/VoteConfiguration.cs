using KWishes.Core.Domain.Votes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KWishes.Core.Application.Votes;

public class VoteConfiguration : IEntityTypeConfiguration<Vote>
{
    public void Configure(EntityTypeBuilder<Vote> builder)
    {
        builder.ToTable("votes")
            .HasKey(x => new {x.CreatorId, x.Type, x.EntityId});

        builder
            .HasOne(y => y.Creator)
            .WithMany()
            .HasForeignKey(e => e.CreatorId)
            .IsRequired();
    }
}