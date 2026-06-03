namespace AuctionService.Model
{
    public class Auction
    {
        public Guid Id { get; set; }
        public int ReservePrice { get; set; } = 0;
        public required string Seller { get; set; }
        public string Winner { get; set; }
        public int SoldAmount { get; set; } = 0;
        public int CurrentHighBid { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }
        public DateTime AuctionEnd { get; set; }

        public AuctionStatus Status { get; set; } = AuctionStatus.Live;
        public Guid PropertyId { get; set; }
        public Property Property { get; set; }
    }
}
