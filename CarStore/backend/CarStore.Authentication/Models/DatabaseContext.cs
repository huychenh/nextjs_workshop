using CarStore.Authentication.Commons;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarStore.Authentication.Models
{
    public class DatabaseContext : IdentityDbContext<ApplicationUser>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole()
                {
                    Id = "asdfghjklmnbvcxzqwertyuio01",
                    Name = CarStoreConstants.SuperUserRole,
                    NormalizedName = CarStoreConstants.SuperUserRole.ToUpper()
                },
                new IdentityRole()
                {
                    Id = "asdfghjklmnbvcxzqwertyuio02",
                    Name = CarStoreConstants.UserRole,
                    NormalizedName = CarStoreConstants.UserRole.ToUpper()
                }
            );
        }

    }
}
