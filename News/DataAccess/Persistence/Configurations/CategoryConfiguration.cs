using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Persistence.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(p => p.Id).HasDefaultValueSql("newid()");

            builder.Property(p => p.Name)
                .IsRequired()
                .HasColumnType("nvarchar(255)");

            builder.Property(p => p.Slug)
                .IsRequired()
                .HasColumnType("varchar(255)");

            builder.Property(p => p.CreatedUserId)
                .HasColumnType("nvarchar(450)")
                .IsRequired();

            builder.Property(p => p.IsHidden)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(p => p.CreatedOn)
                .IsRequired()
                .HasDefaultValueSql("getutcdate()");

            builder.Property(p => p.OrderIndex)
                .IsRequired();

            builder.HasOne(p => p.CreatedUser)
                .WithMany(p => p.Categories)
                .HasForeignKey(p => p.CreatedUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(p => p.Slug).IsUnique();

            builder.Navigation(p => p.CreatedUser).AutoInclude();
        }
    }
}
