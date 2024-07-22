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
    public class TwoFactorAuthConfiguration : IEntityTypeConfiguration<TwoFactorAuth>
    {
        public void Configure(EntityTypeBuilder<TwoFactorAuth> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(p => p.UserId)
                .IsRequired()
                .HasColumnType("nvarchar(450)");

            builder.Property(p => p.Token)
                .IsRequired();

            builder.Property(p => p.Code)
                .IsRequired()
                .HasColumnType("varchar(10)");

            builder.Property(p => p.IsAuthenticated)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(p => p.Expires)
                .IsRequired();

            builder.HasOne(p => p.User)
                .WithMany(p => p.TwoFactorAuths)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
