using AuctionService.Data;
using AuctionService.Model;

namespace SearchService.Data
{
    public class DbInitializer
    {
        public static void Initialize(RealEstateContext context)
        {
            context.Database.EnsureCreated();
            // Check if there are any properties already in the database
            if (context.properties.Any())
            {
                return; // Database has been seeded
            }
            var properties = new Property[]
            {
                new Property
                {
                    Id = Guid.NewGuid(),
                    Title = "Cozy Cottage",
                    Description = "A charming cottage with a beautiful garden.",
                    Address = "123 Main St",
                    City = "Springfield",
                    State = "IL",
                    zipCode = "62704",
                    Country = "USA",
                    Bedrooms = 3,
                    Bathrooms = 2,
                    AreaSqFt = 1500,
                    ImageUrl = "https://example.com/images/cottage.jpg"
                },
                new Property
                {
                    Id = Guid.NewGuid(),
                    Title = "Modern Apartment",
                    Description = "A sleek apartment in the heart of the city.",
                    Address = "456 Elm St",
                    City = "Metropolis",
                    State = "NY",
                    zipCode = "10001",
                    Country = "USA",
                    Bedrooms = 2,
                    Bathrooms = 1,
                    AreaSqFt = 900,
                    ImageUrl = "https://example.com/images/apartment.jpg"
                }
            };
            foreach (var property in properties)
            {
                context.properties.Add(property);
            }
            context.SaveChanges();
        }


        public static void InitializeData(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetService<RealEstateContext>();

            //Initialize

        }
    }
}

