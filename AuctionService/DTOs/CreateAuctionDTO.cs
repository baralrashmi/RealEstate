using AuctionService.Model;

namespace AuctionService.DTOs
{
    public class CreateAuctionDTO
    {

        public int ReservePrice { get; set; } = 0;
        public DateTime AuctionEnd { get; set; }
        public string Seller { get; set; }
        public Property property { get; set; }
    }
}
