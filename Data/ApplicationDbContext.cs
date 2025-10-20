using Microsoft.EntityFrameworkCore;
using RealEstateManagementSystem.Models;

namespace RealEstateManagementSystem.Data
{
    /// <summary>
    /// Application database context for Entity Framework Core
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets for all entities
        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<PropertyImage> PropertyImages { get; set; }
        public DbSet<Inquiry> Inquiries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.FirstName).IsRequired();
                entity.Property(e => e.LastName).IsRequired();
                entity.Property(e => e.PasswordHash).IsRequired();
            });

            // Configure Admin entity
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.HasKey(e => e.AdminId);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.PasswordHash).IsRequired();
            });

            // Configure Property entity
            modelBuilder.Entity<Property>(entity =>
            {
                entity.HasKey(e => e.PropertyId);
                entity.Property(e => e.Title).IsRequired();
                entity.Property(e => e.Description).IsRequired();
                entity.Property(e => e.PropertyType).IsRequired();
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Area).HasColumnType("decimal(18,2)");
                
                // Configure one-to-many relationship with PropertyImages
                entity.HasMany(p => p.PropertyImages)
                    .WithOne(i => i.Property)
                    .HasForeignKey(i => i.PropertyId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Configure one-to-many relationship with Inquiries
                entity.HasMany(p => p.Inquiries)
                    .WithOne(i => i.Property)
                    .HasForeignKey(i => i.PropertyId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Configure PropertyImage entity
            modelBuilder.Entity<PropertyImage>(entity =>
            {
                entity.HasKey(e => e.ImageId);
                entity.Property(e => e.ImageUrl).IsRequired();
            });

            // Configure Inquiry entity
            modelBuilder.Entity<Inquiry>(entity =>
            {
                entity.HasKey(e => e.InquiryId);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.Subject).IsRequired();
                entity.Property(e => e.Message).IsRequired();

                // Configure relationship with User
                entity.HasOne(i => i.User)
                    .WithMany(u => u.Inquiries)
                    .HasForeignKey(i => i.UserId)
                    .OnDelete(DeleteBehavior.SetNull);
            });
        }
    }
}
