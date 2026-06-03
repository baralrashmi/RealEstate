using AuctionService.Model;

namespace AuctionService.Data
{
    public class DbInitializer
    {
        public static void Initialize(RealEstateContext context)
        {
            // Ensure database is created
            context.Database.EnsureCreated();

            // Check if data already exists
            if (context.Auctions.Any())
            {
                return; // DB has been seeded
            }

            var auctions = new List<Auction>
            {
                new Auction 
                {
                    Id = Guid.NewGuid(),
                    ReservePrice = 500000,
                    Seller = "seller1",
                    Status = AuctionStatus.Live,
                    AuctionEnd = DateTime.UtcNow.AddDays(30),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    Property = new Property
                    {
                        Id = Guid.NewGuid(),
                        Title = "Modern Family Home on Main Street",
                        Address = "123 Main Street",
                        City = "New York",
                        State = "NY",
                        zipCode="92121",
                        Country="USA",
                        Bedrooms = 4,
                        Bathrooms = 3,

                        Description = "Beautiful modern home in prime location with spacious rooms and updated kitchen.",
                        ImageUrl = "https://images.unsplash.com/photo-1568605114967-8130f3a36994"
                    }
                },
                new Auction
                {
                    Id = Guid.NewGuid(),
                    ReservePrice = 750000,
                    Seller = "seller2",
                    Status = AuctionStatus.Live,
                    AuctionEnd = DateTime.UtcNow.AddDays(45),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    Property = new Property
                    {
                        Id = Guid.NewGuid(),
                        Title = "Luxury Ocean View Condo",
                        Address = "456 Oak Avenue",
                        City = "Los Angeles",
                        State = "CA",
                        zipCode="92121",
                        Country="USA",
                        Bedrooms = 3,
                        Bathrooms = 2,
                        Description = "Luxury condo with ocean views, modern amenities, and secure parking.",
                        ImageUrl = "https://images.unsplash.com/photo-1545324418-cc1a3fa10c00"
                    }
                },
                new Auction
                {
                    Id = Guid.NewGuid(),
                    ReservePrice = 350000,
                    Seller = "seller3",
                    Status = AuctionStatus.Live,
                    AuctionEnd = DateTime.UtcNow.AddDays(20),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    Property = new Property
                    {
                        Id = Guid.NewGuid(),
                        Title = "Charming Downtown Townhouse",
                        Address = "321 Park Place",
                        City = "Chicago",
                        State = "IL",
                        zipCode="92121",
                        Country="USA",
                        Bedrooms = 3,
                        Bathrooms = 2,
                        Description = "Charming townhouse near downtown with updated interiors and private patio.",
                        ImageUrl = "https://images.unsplash.com/photo-1580587771525-78b9dba3b914"
                    }
                },
                new Auction
                {
                    Id = Guid.NewGuid(),
                    ReservePrice = 950000,
                    Seller = "seller4",
                    Status = AuctionStatus.Live,
                    AuctionEnd = DateTime.UtcNow.AddDays(40),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    Property = new Property
                    {
                        Id = Guid.NewGuid(),
                        Title = "Mountain View Estate",
                        Address = "555 Mountain View Drive",
                        City = "Denver",
                        State = "CO",
                        zipCode="92121",
                        Country="USA",
                        Bedrooms = 4,
                        Bathrooms = 3,
                        Description = "Mountain retreat with panoramic views, high-end finishes, and large deck.",
                        ImageUrl = "https://images.unsplash.com/photo-1600596542815-ffad4c1539a9"
                    }
                },
                new Auction
                {
                    Id = Guid.NewGuid(),
                    ReservePrice = 425000,
                    Seller = "seller5",
                    Status = AuctionStatus.Live,
                    AuctionEnd = DateTime.UtcNow.AddDays(-5),
                    CreatedAt = DateTime.UtcNow.AddDays(-35),
                    UpdatedAt = DateTime.UtcNow,
                    Property = new Property
                    {
                        Id = Guid.NewGuid(),
                        Title = "Urban Condo in Vibrant Neighborhood",
                        Address = "888 Maple Street",
                        City = "Seattle",
                        State = "WA",
                        zipCode="92121",
                        Country="USA",
                        Bedrooms = 2,
                        Bathrooms = 2,
                        Description = "Urban condo in vibrant neighborhood with modern appliances and gym access.",
                        ImageUrl = "https://images.unsplash.com/photo-1512917774080-9991f1c4c750"
                    }
                },
                new Auction
                {
                    Id = Guid.NewGuid(),
                    ReservePrice = 2500000,
                    Seller = "seller6",
                    Status = AuctionStatus.Live,
                    AuctionEnd = DateTime.UtcNow.AddDays(-10),
                    CreatedAt = DateTime.UtcNow.AddDays(-40),
                    UpdatedAt = DateTime.UtcNow.AddDays(-10),
                    Property = new Property
                    {
                        Id = Guid.NewGuid(),
                        Title = "Exclusive Skyline Penthouse",
                        Address = "1000 Skyline Penthouse",
                        City = "San Francisco",
                        State = "CA",
                        zipCode="92121",
                        Country="USA",
                        Bedrooms = 4,
                        Bathrooms = 4,
                        Description = "Exclusive penthouse with 360-degree city views, rooftop terrace, and smart home technology.",
                        ImageUrl = "https://images.unsplash.com/photo-1600607687939-ce8a6c25118c"
                    }
                }
            };

            context.Auctions.AddRange(auctions);
            context.SaveChanges();
        }

        public static void InitializeData(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetService<RealEstateContext>()
                ?? throw new InvalidOperationException("Unable to get RealEstateContext from service provider.");
            Initialize(context);
        }
    }
}

