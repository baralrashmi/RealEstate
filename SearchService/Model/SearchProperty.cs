using AuctionService.Model;

namespace SearchService.Model
{
    public class SearchProperty
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

        public AuctionStatus Status { get; set; }
        public required string Title { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string zipCode { get; set; }
        public string Country { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public decimal AreaSqFt { get; set; }
        public required string ImageUrl { get; set; }
    }
}
