using AuctionService.Model;
using Microsoft.EntityFrameworkCore;


namespace AuctionService.Data
{
    public class RealEstateContext : DbContext
    {
        public RealEstateContext(DbContextOptions<RealEstateContext> options) : base(options)
        {

        }

        //Add DBset
        public DbSet<Auction> Auctions { get; set; }
        public DbSet<Property> Properties { get; set; }

        //Perform any additional configuration if needed
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the relationship between Auction and Property
            modelBuilder.Entity<Auction>()
                .HasOne(a => a.Property)
                .WithOne()
                .HasForeignKey<Auction>(a => a.PropertyId);

            // No store type was specified for the decimal property 'AreaSqFt' on entity type 'Property'.
            //Fix above error by specifying the store type for the decimal property
            modelBuilder.Entity<Property>()
                .Property(p => p.AreaSqFt)
                .HasColumnType("decimal(18,2)");
        }



    }

}
