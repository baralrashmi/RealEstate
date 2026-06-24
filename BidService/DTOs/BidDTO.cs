namespace BidService.DTOs
{
    public class BidDTO
    {
        public Guid Id { get; set; }
        public Guid AuctionId { get; set; }
        public required string BidderName { get; set; }
        public decimal BidAmount { get; set; }
        public DateTime PlacedAt { get; set; }
        public bool IsWinning { get; set; }
    }

    public class CreateBidDTO
    {
        public Guid AuctionId { get; set; }
        public required string BidderName { get; set; }
        public decimal BidAmount { get; set; }
    }
}
