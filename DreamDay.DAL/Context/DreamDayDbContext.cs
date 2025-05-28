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
        public DbSet<WeddingVendor> WeddingVendors { get; set; }
        public DbSet<BudgetItem> BudgetItems { get; set; }
        public DbSet<TimelineEvent> TimelineEvents { get; set; }
        public DbSet<PlannerWedding> PlannerWeddings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User ↔ Tenant (M:1)
            modelBuilder.Entity<User>()
                .HasOne(u => u.Tenant)
                .WithMany(u => u.Users)
                .HasForeignKey(u => u.TenantId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_User_Tenant");

            // Wedding ↔ Owner (User) (M:1)
            modelBuilder.Entity<Wedding>()
                .HasOne(w => w.Owner)
                .WithMany(u => u.Weddings)
                .HasForeignKey(w => w.OwnerId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Wedding_Owner");

            // Wedding ↔ CreatedBy (User) (M:1)
            modelBuilder.Entity<Wedding>()
                .HasOne(w => w.CreatedBy)
                .WithMany()
                .HasForeignKey(w => w.CreatedByUserId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Wedding_CreatedBy");

            // Wedding ↔ Venue (M:1)
            modelBuilder.Entity<Wedding>()
                .HasOne(w => w.Venue)
                .WithMany(v => v.Weddings)
                .HasForeignKey(w => w.VenueId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Wedding_Venue");

            // Wedding ↔ WeddingChecklistItem (1:M)
            modelBuilder.Entity<WeddingChecklistItem>()
                .HasOne(c => c.Wedding)
                .WithMany(w => w.WeddingChecklistItems)
                .HasForeignKey(c => c.WeddingId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_ChecklistItem_Wedding");

            // Wedding ↔ Guest (1:M)
            modelBuilder.Entity<Guest>()
                .HasOne(g => g.Wedding)
                .WithMany(w => w.Guests)
                .HasForeignKey(g => g.WeddingId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Guest_Wedding");

            // Wedding ↔ BudgetItem (1:M)
            modelBuilder.Entity<BudgetItem>()
                .HasOne(b => b.Wedding)
                .WithMany(w => w.BudgetItems)
                .HasForeignKey(b => b.WeddingId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_BudgetItem_Wedding");

            // Wedding ↔ TimelineEvent (1:M)
            modelBuilder.Entity<TimelineEvent>()
                .HasOne(e => e.Wedding)
                .WithMany(w => w.TimelineEvents)
                .HasForeignKey(e => e.WeddingId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_TimelineEvent_Wedding");

            // WeddingVendor (Wedding ↔ Vendor) (M:M)
            modelBuilder.Entity<WeddingVendor>()
                .HasKey(wv => new { wv.WeddingId, wv.VendorId });

            modelBuilder.Entity<WeddingVendor>()
                .HasOne(wv => wv.Wedding)
                .WithMany(w => w.WeddingVendors)
                .HasForeignKey(wv => wv.WeddingId)
                .HasConstraintName("FK_WeddingVendor_Wedding");

            modelBuilder.Entity<WeddingVendor>()
                .HasOne(wv => wv.Vendor)
                .WithMany(v => v.WeddingVendors)
                .HasForeignKey(wv => wv.VendorId)
                .HasConstraintName("FK_WeddingVendor_Vendor");

            // PlannerWedding (User ↔ Wedding) (M:M)
            modelBuilder.Entity<PlannerWedding>()
                .HasKey(pw => new { pw.PlannerId, pw.WeddingId });

            modelBuilder.Entity<PlannerWedding>()
                .HasOne(pw => pw.Planner)
                .WithMany(u => u.PlannerWeddings)
                .HasForeignKey(pw => pw.PlannerId)
                .HasConstraintName("FK_PlannerWedding_Planner");

            modelBuilder.Entity<PlannerWedding>()
                .HasOne(pw => pw.Wedding)
                .WithMany(w => w.PlannerWeddings)
                .HasForeignKey(pw => pw.WeddingId)
                .HasConstraintName("FK_PlannerWedding_Wedding");
        }
    }
}
