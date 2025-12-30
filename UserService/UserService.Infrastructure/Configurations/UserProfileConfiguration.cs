using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Configurations;

public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.ToTable("UserProfiles");
        builder.HasOne(profile => profile.User)
            .WithOne(user => user.UserProfile)
            .HasForeignKey<UserProfile>(profile => profile.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}