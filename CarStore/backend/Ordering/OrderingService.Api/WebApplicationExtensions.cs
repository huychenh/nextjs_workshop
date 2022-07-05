using Microsoft.EntityFrameworkCore;
using OrderingService.Infrastructure.Data;

namespace OrderingService.Api
{
    public static class WebApplicationExtensions
    {
        public static WebApplication MigrateDatabase(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<MainDbContext>();
                db.Database.Migrate();
            }

            return app;
        }
    }
}
