using Microsoft.EntityFrameworkCore;
using RentalService.API.Models;

namespace RentalService.API.Data;

public class RentalDbContext : DbContext
{
    public RentalDbContext(DbContextOptions<RentalDbContext> options) : base(options)
    {
    }

    public DbSet<Rental> Rentals { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Rental>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.BookingNumber)
                .IsRequired()
                .HasDefaultValueSql("NEWID()");

            entity.HasIndex(e => e.BookingNumber)
                .IsUnique();

            entity.Property(e => e.RegistrationNumber)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(e => e.CustomerSocialSecurityNumber)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(e => e.Category)
                .IsRequired()
                .HasConversion<int>();

            entity.Property(e => e.PickupDateTime)
                .IsRequired();

            entity.Property(e => e.PickupMeterReading)
                .IsRequired();

            entity.Property(e => e.ReturnDateTime);

            entity.Property(e => e.ReturnMeterReading);

            entity.Property(e => e.CalculatedPrice)
                .HasColumnType("decimal(18,2)");

            entity.Property(e => e.Status)
                .IsRequired()
                .HasConversion<int>();

            entity.Property(e => e.CreatedAt)
                .IsRequired();

            entity.Property(e => e.UpdatedAt);
        });
    }
}
