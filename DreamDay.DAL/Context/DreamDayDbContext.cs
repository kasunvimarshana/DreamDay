using DreamDay.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.DAL.Context
{
    public class DreamDayDbContext : DbContext
    {
        public DreamDayDbContext(DbContextOptions<DreamDayDbContext> options) : base(options) { }

        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Venue> Venues { get; set; }
        public DbSet<Wedding> Weddings { get; set; }
        public DbSet<WeddingChecklistItem> WeddingChecklistItems { get; set; }
        public DbSet<Guest> Guests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Tenant)
                .WithMany(t => t.Users)
                .HasForeignKey(u => u.TenantId)
                .IsRequired(false); // Makes TenantId optional

            modelBuilder.Entity<Wedding>()
                .HasOne(w => w.User)
                .WithMany(u => u.Weddings)
                .HasForeignKey(w => w.UserId);

            modelBuilder.Entity<WeddingChecklistItem>()
                .HasOne(c => c.Wedding)
                .WithMany(w => w.WeddingChecklistItems)
                .HasForeignKey(c => c.WeddingId);

            modelBuilder.Entity<Guest>()
                .HasOne(g => g.Wedding)
                .WithMany(w => w.Guests)
                .HasForeignKey(g => g.WeddingId);
        }
    }
}
