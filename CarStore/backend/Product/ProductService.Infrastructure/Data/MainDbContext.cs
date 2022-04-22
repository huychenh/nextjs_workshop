using Microsoft.EntityFrameworkCore;
using N8T.Infrastructure.EfCore;
using ProductService.AppCore.Core;

namespace ProductService.Infrastructure.Data
{
    public class MainDbContext : AppDbContextBase
    {
        public MainDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; } = default!;

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
        }
    }
}
