

using Microsoft.EntityFrameworkCore;

namespace SearchService.Data
{
    public class DbInitializer
    {

        public static async Task InitDb(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetService<SearchDbContext>();
            if (context != null)
            {
                await context.Database.MigrateAsync();
            }
        }

    }

}
