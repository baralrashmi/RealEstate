

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

            // Check if database already has data to prevent duplicate insertions
            if (await context.SearchProperties.AnyAsync())
            {
                Console.WriteLine("Database already contains data - skipping initialization");
                return;
            }

            var auctionServiceClient = scope.ServiceProvider.GetRequiredService<AuctionServiceHttpClient>();
            var items = await auctionServiceClient.GetItemsforSearchAsync();

            if (items.Count > 0)
            {
                Console.WriteLine($"Seeding database with {items.Count} items");
                context.SearchProperties.AddRange(items);
                await context.SaveChangesAsync();
            }
            else
            {
                Console.WriteLine("No items returned from AuctionService - skipping initialization");
            }

        }

    }
}
