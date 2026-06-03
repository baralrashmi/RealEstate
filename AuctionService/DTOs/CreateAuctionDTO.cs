using AuctionService.Model;

namespace AuctionService.DTOs
{
    public class CreateAuctionDTO
    {

        public int ReservePrice { get; set; } = 0;
        public DateTime AuctionEnd { get; set; }
        public string Seller { get; set; }
        public Property Property { get; set; }


        //Add a method to convert CreateAuctionDTO to Auction entity
        public Auction ToAuction()
        {
            return new Auction
            {
                ReservePrice = this.ReservePrice,
                AuctionEnd = this.AuctionEnd,
                Seller = this.Seller,
                Property = this.Property
            };
        }

        //Add A method to convert Auction entity to CreateAuctionDTO
        public static CreateAuctionDTO FromAuction(Auction auction)
        {
            return new CreateAuctionDTO
            {
                ReservePrice = auction.ReservePrice,
                AuctionEnd = auction.AuctionEnd,
                Seller = auction.Seller,
                Property = auction.Property
            };
        }
    }
}
