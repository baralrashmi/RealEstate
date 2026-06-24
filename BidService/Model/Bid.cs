namespace BidService.Model
{
    public class Bid
    {
        public Guid Id { get; set; }
        public Guid AuctionId { get; set; }
        public required string BidderName { get; set; }
        public decimal BidAmount { get; set; }
        public DateTime PlacedAt { get; set; } = DateTime.UtcNow;
        public bool IsWinning { get; set; } = false;
    }
}
