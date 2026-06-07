

using Microsoft.EntityFrameworkCore;
using SearchService.Service;

namespace SearchService.Data
{
    public class DbInitializer
    {

        public static async Task InitDb(WebApplication app)
        {
            //Marked as scoped to ensure that the database context is properly disposed after use
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<SearchDbContext>();
            
                // Apply any pending migrations to the database
              //  await context.Database.MigrateAsync();
            
            var auctionServiceClient = scope.ServiceProvider.GetRequiredService<AuctionServiceHttpClient>();
            var items = await auctionServiceClient.GetItemsforSearchAsync();

            if (items.Count > 0)
            {
                context.SearchProperties.AddRange(items);
                await context.SaveChangesAsync();
            }

        }

    }
}
