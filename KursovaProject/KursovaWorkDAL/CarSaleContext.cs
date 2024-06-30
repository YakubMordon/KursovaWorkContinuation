using Microsoft.EntityFrameworkCore;
using KursovaWork.Infrastructure.Services.DB;
using KursovaWork.Domain.Entities.Car;
using KursovaWork.Domain.Entities;

namespace KursovaWork.Infrastructure;

/// <summary>
/// Database context for car sales.
/// </summary>
public class CarSaleContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the CarSaleContext class with the specified options.
    /// </summary>
    /// <param name="options">Database options.</param>
    public CarSaleContext(DbContextOptions<CarSaleContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Initializes a new instance of the CarSaleContext class.
    /// </summary>
    public CarSaleContext() : base() { }

    /// <summary>
    /// Represents the table of cars in the database.
    /// </summary>
    public DbSet<Car> Cars { get; set; }

    /// <summary>
    /// Represents the table of car images in the database.
    /// </summary>
    public DbSet<CarImage> CarImages { get; set; }

    /// <summary>
    /// Represents the table of car details in the database.
    /// </summary>
    public DbSet<CarDetail> CarDetails { get; set; }

    /// <summary>
    /// Represents the table of users in the database.
    /// </summary>
    public DbSet<User> Users { get; set; }

    /// <summary>
    /// Represents the table of car orders in the database.
    /// </summary>
    public DbSet<Order> Orders { get; set; }

    /// <summary>
    /// Represents the table of user credit cards in the database.
    /// </summary>
    public DbSet<Card> Cards { get; set; }

    /// <summary>
    /// Represents the table of configurator options for cars in the database.
    /// </summary>
    public DbSet<ConfiguratorOptions> ConfiguratorOptions { get; set; }

    /// <summary>
    /// Defines the database model and establishes relationships between tables.
    /// </summary>
    /// <param name="modelBuilder">Object used to build the model.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Car>()
            .HasMany(c => c.Images)
            .WithOne(ci => ci.Car)
            .HasForeignKey(ci => ci.CarId);

        modelBuilder.Entity<Car>()
            .HasOne(c => c.Detail)
            .WithOne(cd => cd.Car)
            .HasForeignKey<CarDetail>(cd => cd.CarId);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.User)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.UserId);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.Car)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.CarId);

        // Adding encryption for credit card
        modelBuilder.Entity<Card>()
            .Property(o => o.CardNumber)
            .HasConversion(
                card => Encrypter.Encrypt(card),
                encryptedCard => Encrypter.Decrypt(encryptedCard)
            );

        // Adding encryption for month
        modelBuilder.Entity<Card>()
            .Property(o => o.ExpirationMonth)
            .HasConversion(
                month => Encrypter.EncryptMonth(month),
                encryptedMonth => Encrypter.DecryptMonth(encryptedMonth)
            );

        // Adding encryption for year
        modelBuilder.Entity<Card>()
            .Property(o => o.ExpirationYear)
            .HasConversion(
                year => Encrypter.EncryptYear(year),
                encryptedYear => Encrypter.DecryptYear(encryptedYear)
            );

        // Adding encryption for CVV code
        modelBuilder.Entity<Card>()
            .Property(o => o.Cvv)
            .HasConversion(
                cvv => Encrypter.EncryptCvv(cvv),
                encryptedCvv => Encrypter.DecryptCvv(encryptedCvv)
            );

        // Adding encryption for password
        modelBuilder.Entity<User>()
            .Property(u => u.Password)
            .HasConversion(
                password => Encrypter.HashPassword(password),
                hashedPassword => hashedPassword
            );

        modelBuilder.Entity<User>()
            .Property(u => u.ConfirmPassword)
            .HasConversion(
                password => Encrypter.HashPassword(password),
                hashedPassword => hashedPassword
            );

        base.OnModelCreating(modelBuilder);
    }

    /// <summary>
    /// Populates the database with initial data.
    /// </summary>
    public void FillDb()
    {
        DbInitializer.Initialize(this);
    }
}