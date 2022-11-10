using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using N8T.Infrastructure.EfCore;
using ProductService.AppCore.Core;

namespace ProductService.Infrastructure.Data
{
    public class MainDbContext : AppDbContextBase, IDbFacadeResolver
    {
        public MainDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; } = default!;
        public DbSet<Brand> Brands { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension(Consts.UuidGenerator);

            // product
            modelBuilder.Entity<Product>().HasKey(x => x.Id);
            modelBuilder.Entity<Product>().Property(x => x.Id).HasColumnType("uuid")
                .HasDefaultValueSql(Consts.UuidAlgorithm);

            modelBuilder.Entity<Product>().Property(x => x.Created).HasDefaultValueSql(Consts.DateAlgorithm);

            modelBuilder.Entity<Product>().HasIndex(x => x.Id).IsUnique();
            modelBuilder.Entity<Product>().Ignore(x => x.DomainEvents);

            var splitStringConverter = new ValueConverter<ICollection<string>, string?>(
                v => v != null && v.Any() ? string.Join(";", v) : null,
                v => !string.IsNullOrEmpty(v) ? v.Split(new[] { ';' }) : Array.Empty<string>());

            modelBuilder.Entity<Product>()
                   .Property(nameof(Product.Images))
                   .HasConversion(splitStringConverter);

            // brand
            modelBuilder.Entity<Brand>().HasKey(x => x.Id);
            modelBuilder.Entity<Brand>().Property(x => x.Id).HasColumnType("uuid")
                .HasDefaultValueSql(Consts.UuidAlgorithm);
            modelBuilder.Entity<Brand>().Property(x => x.Name).HasMaxLength(20);
            modelBuilder.Entity<Brand>().Property(x => x.Created).HasDefaultValueSql(Consts.DateAlgorithm);

            modelBuilder.Entity<Brand>().HasIndex(x => x.Id).IsUnique();
            modelBuilder.Entity<Brand>().HasIndex(x => x.Name).IsUnique();

            modelBuilder.Entity<Brand>().Ignore(x => x.DomainEvents);
            modelBuilder.Entity<Brand>()
                .HasMany(x => x.Products)
                .WithOne(x => x.Brand)
                .IsRequired();
        }
    }
}
