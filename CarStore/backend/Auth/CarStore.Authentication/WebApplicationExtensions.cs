using CarStore.Authentication.Models;
using Microsoft.EntityFrameworkCore;

namespace CarStore.Authentication
{
    public static class WebApplicationExtensions
    {
        public static WebApplication MigrateDatabase(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                db.Database.Migrate();
            }

            return app;
        }
    }
}
