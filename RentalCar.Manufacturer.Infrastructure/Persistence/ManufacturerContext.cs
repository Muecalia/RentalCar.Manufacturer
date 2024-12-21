using Microsoft.EntityFrameworkCore;
using RentalCar.Manufacturer.Core.Entities;

namespace RentalCar.Manufacturer.Infrastructure.Persistence;

public class ManufacturerContext : DbContext
{
    public ManufacturerContext(DbContextOptions<ManufacturerContext> options) : base(options) { }
    public DbSet<Manufacturers> Manufacturer { get; set; }
    

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Manufacturers>(e => 
        {
            e.HasKey(c => c.Id);
            
            e.Property<string>(c => c.Name)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(50);
            
            e.Property<string>(c => c.Email)
                .IsUnicode()
                .HasMaxLength(100);
            
            e.Property<string>(c => c.Phone)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(25);
            
            e.HasIndex(c => c.Name).IsUnique();
            e.HasIndex(c => c.Email).IsUnique();
            e.HasIndex(c => c.Phone);
        });

        base.OnModelCreating(builder);
    }
}