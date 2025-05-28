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

            // **TENANT RELATIONSHIPS**
            // Tenant -> Users (One-to-Many)
            modelBuilder.Entity<Tenant>()
                .HasMany(t => t.Users)
                .WithOne(u => u.Tenant)
                .HasForeignKey(u => u.TenantId)
                .OnDelete(DeleteBehavior.Restrict);

            // **USER RELATIONSHIPS**
            // User -> Weddings (One-to-Many as Owner)
            modelBuilder.Entity<User>()
                .HasMany(u => u.Weddings)
                .WithOne(w => w.Owner)
                .HasForeignKey(w => w.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            // User -> User (Self-referencing for CreatedBy)
            modelBuilder.Entity<User>()
                .HasOne(u => u.CreatedBy)
                .WithMany()
                .HasForeignKey(u => u.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // **VENDOR RELATIONSHIPS**
            // Tenant -> Vendors (One-to-Many)
            modelBuilder.Entity<Tenant>()
                .HasMany<Vendor>()
                .WithOne(v => v.Tenant)
                .HasForeignKey(v => v.TenantId)
                .OnDelete(DeleteBehavior.Restrict);

            // User -> Vendors (One-to-Many as Creator)
            modelBuilder.Entity<User>()
                .HasMany<Vendor>()
                .WithOne(v => v.CreatedBy)
                .HasForeignKey(v => v.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // **VENUE RELATIONSHIPS**
            // Tenant -> Venues (One-to-Many)
            modelBuilder.Entity<Tenant>()
                .HasMany<Venue>()
                .WithOne(v => v.Tenant)
                .HasForeignKey(v => v.TenantId)
                .OnDelete(DeleteBehavior.Restrict);

            // User -> Venues (One-to-Many as Creator)
            modelBuilder.Entity<User>()
                .HasMany<Venue>()
                .WithOne(v => v.CreatedBy)
                .HasForeignKey(v => v.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Venue -> Weddings (One-to-Many)
            modelBuilder.Entity<Venue>()
                .HasMany(v => v.Weddings)
                .WithOne(w => w.Venue)
                .HasForeignKey(w => w.VenueId)
                .OnDelete(DeleteBehavior.Restrict);

            // **WEDDING RELATIONSHIPS**
            // Tenant -> Weddings (One-to-Many)
            modelBuilder.Entity<Tenant>()
                .HasMany<Wedding>()
                .WithOne(w => w.Tenant)
                .HasForeignKey(w => w.TenantId)
                .OnDelete(DeleteBehavior.Restrict);

            // User -> Weddings (One-to-Many as Creator)
            modelBuilder.Entity<User>()
                .HasMany<Wedding>()
                .WithOne(w => w.CreatedBy)
                .HasForeignKey(w => w.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Wedding -> WeddingChecklistItems (One-to-Many)
            modelBuilder.Entity<Wedding>()
                .HasMany(w => w.WeddingChecklistItems)
                .WithOne(wci => wci.Wedding)
                .HasForeignKey(wci => wci.WeddingId)
                .OnDelete(DeleteBehavior.Cascade);

            // Wedding -> Guests (One-to-Many)
            modelBuilder.Entity<Wedding>()
                .HasMany(w => w.Guests)
                .WithOne(g => g.Wedding)
                .HasForeignKey(g => g.WeddingId)
                .OnDelete(DeleteBehavior.Cascade);

            // Wedding -> BudgetItems (One-to-Many)
            modelBuilder.Entity<Wedding>()
                .HasMany(w => w.BudgetItems)
                .WithOne(bi => bi.Wedding)
                .HasForeignKey(bi => bi.WeddingId)
                .OnDelete(DeleteBehavior.Cascade);

            // Wedding -> TimelineEvents (One-to-Many)
            modelBuilder.Entity<Wedding>()
                .HasMany(w => w.TimelineEvents)
                .WithOne(te => te.Wedding)
                .HasForeignKey(te => te.WeddingId)
                .OnDelete(DeleteBehavior.Cascade);

            // **WEDDING CHECKLIST ITEM RELATIONSHIPS**
            // Tenant -> WeddingChecklistItems (One-to-Many)
            modelBuilder.Entity<Tenant>()
                .HasMany<WeddingChecklistItem>()
                .WithOne(wci => wci.Tenant)
                .HasForeignKey(wci => wci.TenantId)
                .OnDelete(DeleteBehavior.Restrict);

            // User -> WeddingChecklistItems (One-to-Many as Creator)
            modelBuilder.Entity<User>()
                .HasMany<WeddingChecklistItem>()
                .WithOne(wci => wci.CreatedBy)
                .HasForeignKey(wci => wci.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // **GUEST RELATIONSHIPS**
            // Tenant -> Guests (One-to-Many)
            modelBuilder.Entity<Tenant>()
                .HasMany<Guest>()
                .WithOne(g => g.Tenant)
                .HasForeignKey(g => g.TenantId)
                .OnDelete(DeleteBehavior.Restrict);

            // User -> Guests (One-to-Many as Creator)
            modelBuilder.Entity<User>()
                .HasMany<Guest>()
                .WithOne(g => g.CreatedBy)
                .HasForeignKey(g => g.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // **BUDGET ITEM RELATIONSHIPS**
            // Tenant -> BudgetItems (One-to-Many)
            modelBuilder.Entity<Tenant>()
                .HasMany<BudgetItem>()
                .WithOne(bi => bi.Tenant)
                .HasForeignKey(bi => bi.TenantId)
                .OnDelete(DeleteBehavior.Restrict);

            // User -> BudgetItems (One-to-Many as Creator)
            modelBuilder.Entity<User>()
                .HasMany<BudgetItem>()
                .WithOne(bi => bi.CreatedBy)
                .HasForeignKey(bi => bi.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // **TIMELINE EVENT RELATIONSHIPS**
            // Tenant -> TimelineEvents (One-to-Many)
            modelBuilder.Entity<Tenant>()
                .HasMany<TimelineEvent>()
                .WithOne(te => te.Tenant)
                .HasForeignKey(te => te.TenantId)
                .OnDelete(DeleteBehavior.Restrict);

            // User -> TimelineEvents (One-to-Many as Creator)
            modelBuilder.Entity<User>()
                .HasMany<TimelineEvent>()
                .WithOne(te => te.CreatedBy)
                .HasForeignKey(te => te.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // **MANY-TO-MANY RELATIONSHIPS**

            // Wedding <-> Vendor (Many-to-Many via WeddingVendor)
            modelBuilder.Entity<WeddingVendor>()
                .HasKey(wv => wv.Id);

            modelBuilder.Entity<WeddingVendor>()
                .HasOne(wv => wv.Wedding)
                .WithMany(w => w.WeddingVendors)
                .HasForeignKey(wv => wv.WeddingId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WeddingVendor>()
                .HasOne(wv => wv.Vendor)
                .WithMany(v => v.WeddingVendors)
                .HasForeignKey(wv => wv.VendorId)
                .OnDelete(DeleteBehavior.Cascade);

            // Tenant -> WeddingVendors (One-to-Many)
            modelBuilder.Entity<Tenant>()
                .HasMany<WeddingVendor>()
                .WithOne(wv => wv.Tenant)
                .HasForeignKey(wv => wv.TenantId)
                .OnDelete(DeleteBehavior.Restrict);

            // User -> WeddingVendors (One-to-Many as Creator)
            modelBuilder.Entity<User>()
                .HasMany<WeddingVendor>()
                .WithOne(wv => wv.CreatedBy)
                .HasForeignKey(wv => wv.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // User <-> Wedding (Many-to-Many via PlannerWedding)
            modelBuilder.Entity<PlannerWedding>()
                .HasKey(pw => pw.Id);

            modelBuilder.Entity<PlannerWedding>()
                .HasOne(pw => pw.Planner)
                .WithMany(u => u.PlannerWeddings)
                .HasForeignKey(pw => pw.PlannerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PlannerWedding>()
                .HasOne(pw => pw.Wedding)
                .WithMany(w => w.PlannerWeddings)
                .HasForeignKey(pw => pw.WeddingId)
                .OnDelete(DeleteBehavior.Cascade);

            // Tenant -> PlannerWeddings (One-to-Many)
            modelBuilder.Entity<Tenant>()
                .HasMany<PlannerWedding>()
                .WithOne(pw => pw.Tenant)
                .HasForeignKey(pw => pw.TenantId)
                .OnDelete(DeleteBehavior.Restrict);

            // User -> PlannerWeddings (One-to-Many as Creator)
            modelBuilder.Entity<User>()
                .HasMany<PlannerWedding>()
                .WithOne(pw => pw.CreatedBy)
                .HasForeignKey(pw => pw.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // **ADDITIONAL FIELD CONFIGURATIONS**

            // Default values for timestamps
            modelBuilder.Entity<Tenant>()
                .Property(t => t.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            // **INDEXES FOR PERFORMANCE**

            // Multi-tenant indexes
            modelBuilder.Entity<User>()
                .HasIndex(u => u.TenantId);

            modelBuilder.Entity<Wedding>()
                .HasIndex(w => w.TenantId);

            modelBuilder.Entity<Vendor>()
                .HasIndex(v => v.TenantId);

            modelBuilder.Entity<Venue>()
                .HasIndex(v => v.TenantId);

            // Foreign key indexes
            modelBuilder.Entity<Wedding>()
                .HasIndex(w => w.OwnerId);

            modelBuilder.Entity<Wedding>()
                .HasIndex(w => w.VenueId);

            modelBuilder.Entity<WeddingChecklistItem>()
                .HasIndex(wci => wci.WeddingId);

            modelBuilder.Entity<Guest>()
                .HasIndex(g => g.WeddingId);

            modelBuilder.Entity<BudgetItem>()
                .HasIndex(bi => bi.WeddingId);

            modelBuilder.Entity<TimelineEvent>()
                .HasIndex(te => te.WeddingId);

            // Composite indexes for many-to-many relationships
            modelBuilder.Entity<WeddingVendor>()
                .HasIndex(wv => new { wv.WeddingId, wv.VendorId })
                .IsUnique();

            modelBuilder.Entity<PlannerWedding>()
                .HasIndex(pw => new { pw.PlannerId, pw.WeddingId })
                .IsUnique();

            // Email index for users (if used for login)
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email);
        }
    }
}
