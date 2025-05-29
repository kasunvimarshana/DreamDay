using DreamDay.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.DAL.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasOne(u => u.CreatedBy)
                   .WithMany(u => u.CreatedUsers)
                   .HasForeignKey(u => u.CreatedByUserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.BrideOfWeddings)
                   .WithOne(w => w.Bride)
                   .HasForeignKey(w => w.BrideId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.GroomOfWeddings)
                   .WithOne(w => w.Groom)
                   .HasForeignKey(w => w.GroomId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
