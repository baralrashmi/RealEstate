namespace AuctionService.Model
{
    public class Property
    {
        public Guid Id { get; set; }
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
