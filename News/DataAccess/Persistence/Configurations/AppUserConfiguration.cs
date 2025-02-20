﻿using Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Persistence.Configurations
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.Property(p => p.FirstName).IsRequired()
                .HasMaxLength(35)
                .HasColumnType("nvarchar");

            builder.Property(p => p.LastName).IsRequired()
                .HasMaxLength(35)
                .HasColumnType("nvarchar");

            builder.Property(p => p.Email).IsRequired();

            builder.Property(p => p.AvatarId).IsRequired(false);

            builder.Property(p => p.CreatedDate).HasDefaultValue(DateTime.UtcNow);

            builder.Property(p => p.IsDisabled)
                .IsRequired()
                .HasDefaultValue(false);

            builder.HasOne(p => p.Avatar).WithOne(p => p.Avatar);

            builder.Navigation(p => p.Avatar).AutoInclude();
        }
    }
}
