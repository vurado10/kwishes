using KWishes.Core.Domain.Comments;
using KWishes.Core.Domain.Users;
using KWishes.Core.Domain.Wishes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KWishes.Core.Application.Comments;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable("comments");

        builder.OwnsOne(
            comment => comment.Creator,
            innerBuilder =>
            {
                innerBuilder
                    .Property(c => c.Id)
                    .HasConversion(i => i.Value, g => new UserId(g));
                
                innerBuilder
                    .HasOne<User>()
                    .WithMany()
                    .HasForeignKey(c => c.Id)
                    .HasConstraintName("fk_comments_users_comment_creator_id");
            });

        builder
            .Property(u => u.Id)
            .HasConversion(id => id.Value, guid => new CommentId(guid))
            .ValueGeneratedOnAdd();

        builder
            .HasOne<Wish>()
            .WithMany()
            .HasForeignKey(c => c.WishId)
            .HasConstraintName("fk_comments_wishes_wish_id");
        
        builder
            .Property(c => c.Files)
            .HasColumnType("jsonb");
    }
}
