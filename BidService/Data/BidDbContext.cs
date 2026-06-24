using BidService.Model;
using Microsoft.EntityFrameworkCore;

namespace BidService.Data
{
    public class BidDbContext : DbContext
    {
        public BidDbContext(DbContextOptions<BidDbContext> options) : base(options)
        {
        }

        public DbSet<Bid> Bids { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Bid>()
                .HasKey(b => b.Id);

            modelBuilder.Entity<Bid>()
                .Property(b => b.AuctionId)
                .IsRequired();

            modelBuilder.Entity<Bid>()
                .Property(b => b.BidderName)
                .IsRequired()
                .HasMaxLength(255);

            modelBuilder.Entity<Bid>()
                .Property(b => b.BidAmount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Bid>()
                .HasIndex(b => b.AuctionId);

            modelBuilder.Entity<Bid>()
                .HasIndex(b => new { b.AuctionId, b.PlacedAt });
        }
    }
}
