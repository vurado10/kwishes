using KWishes.Core.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KWishes.Core.Application.Users.Repository;

public class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        
        builder
            .Property(u => u.Id)
            .HasConversion(id => id.Value, guid => new UserId(guid))
            .ValueGeneratedOnAdd();

        builder
            .Property(u => u.Email)
            .HasConversion(address => address.Value, s => new EmailAddress(s));
        
        builder
            .Property(u => u.Avatar)
            .HasConversion(
                uri => ReferenceEquals(uri, null) ? null : uri.AbsoluteUri, 
                s => string.IsNullOrEmpty(s) ? null : new Uri(s));

        builder
            .HasIndex(user => user.Email)
            .HasDatabaseName("ix_email_1")
            .IsUnique();
        
        builder
            .HasIndex(user => user.Username)
            .HasDatabaseName("ix_username_1")
            .IsUnique();
        
        builder
            .HasIndex(user => user.GoogleNameId)
            .HasDatabaseName("ix_google_name_id_1")
            .IsUnique();
    }
}