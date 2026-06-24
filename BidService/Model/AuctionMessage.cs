namespace BidService.Model
{
    public class AuctionMessage
    {
        public Guid AuctionId { get; set; }
        public required string Seller { get; set; }
        public int ReservePrice { get; set; }
        public DateTime AuctionEnd { get; set; }
        public string Status { get; set; } = "Live";
    }
}
