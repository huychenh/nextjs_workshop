using Microsoft.EntityFrameworkCore;
using N8T.Infrastructure.EfCore;
using OrderingService.AppCore.Core;

namespace OrderingService.Infrastructure.Data
{
    public class MainDbContext : AppDbContextBase, IDbFacadeResolver
    {
        public MainDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Order> Orders { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension(Consts.UuidGenerator);

            modelBuilder.Entity<Order>().HasKey(x => x.Id);
            modelBuilder.Entity<Order>().Property(x => x.Id).HasColumnType("uuid")
                .HasDefaultValueSql(Consts.UuidAlgorithm);

            modelBuilder.Entity<Order>().Property(x => x.Created).HasDefaultValueSql(Consts.DateAlgorithm);

            modelBuilder.Entity<Order>().HasIndex(x => x.Id).IsUnique();
            modelBuilder.Entity<Order>().Ignore(x => x.DomainEvents);
        }
    }
}
