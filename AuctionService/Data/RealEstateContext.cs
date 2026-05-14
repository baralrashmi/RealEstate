using AuctionService.Model;
using Microsoft.EntityFrameworkCore;


namespace AuctionService.Data
{
    public class        RealEstateContext : DbContext
    {
        public RealEstateContext(DbContextOptions<RealEstateContext> options) : base(options) 
        {
        
        }

        //Add DBset
        public DbSet<Auction> auctions { get; set; }
        public DbSet<Property> properties { get; set; }

        //Perform any additional configuration if needed
            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);

                // Configure the relationship between Auction and Property
                modelBuilder.Entity<Auction>()
                    .HasOne(a => a.property)
                    .WithOne()
                    .HasForeignKey<Auction>(a => a.PropertyId);                     
        }



    }
}
