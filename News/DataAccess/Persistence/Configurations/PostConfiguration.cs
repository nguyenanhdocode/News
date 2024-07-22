using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Persistence.Configurations
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasDefaultValueSql("newid()");

            builder.Property(p => p.Title).IsRequired();
            builder.Property(p => p.Slug).IsRequired();
            builder.Property(p => p.ViewCount).IsRequired().HasDefaultValue(0);
            builder.Property(p => p.Content).IsRequired();
            builder.Property(p => p.IsPinned).IsRequired().HasDefaultValue(false);
            builder.Property(p => p.UpdatedUserId).IsRequired(false);
            builder.Property(p => p.CreatedOn).HasDefaultValueSql("getutcdate()");
            builder.Property(p => p.CoverPhotoId).IsRequired();

            builder.HasOne(p => p.Category).WithMany(p => p.Posts)
                .HasForeignKey(p => p.CategoryId);

            builder.HasOne(p => p.CreatedUser).WithMany(p => p.Posts)
                .HasForeignKey(p =>p.CreatedUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.UpdatedUser).WithMany(p => p.UpdatedPosts)
                .HasForeignKey(p => p.UpdatedUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.CoverPhoto).WithMany(p => p.CoverPhotos)
                .HasForeignKey(p => p.CoverPhotoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Navigation(p => p.CreatedUser).AutoInclude();
            builder.Navigation(p => p.CoverPhoto).AutoInclude();
            builder.Navigation(p => p.Category).AutoInclude();
        }
    }
}
